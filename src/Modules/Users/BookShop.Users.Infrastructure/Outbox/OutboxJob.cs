using System.Data;
using System.Data.Common;
using System.Text.Json;
using BuildingBlocks.Application.Data;
using BuildingBlocks.Infrastructure.Outbox;
using Dapper;
using Mediator;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TickerQ.Utilities.Base;
using TickerQ.Utilities.Interfaces;

namespace BookShop.Users.Infrastructure.Outbox;

public partial class OutboxJob(
    IDbConnectionFactory dbConnectionFactory,
    IPublisher publisher,
    IOptions<OutboxJobOptions> outboxOptions,
    TimeProvider timeProvider,
    ILogger<OutboxJob> logger
) : ITickerFunction
{
    public async Task ExecuteAsync(TickerFunctionContext context, CancellationToken cancellationToken = default)
    {
        LogServiceBeginningToProcessOutboxMessages(outboxOptions.Value.ServiceName);

        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();
        await using DbTransaction transaction = await connection.BeginTransactionAsync(cancellationToken);

        IReadOnlyList<OutboxMessageResponse> outboxMessages = await GetOutboxMessagesAsync(connection, transaction);

        foreach (OutboxMessageResponse outboxMessage in outboxMessages)
        {
            Exception? exception = null;
            try
            {
                Type messageType = Domain.AssemblyReference.Assembly.GetType(outboxMessage.Type)!;
                object domainEvent = JsonSerializer.Deserialize(outboxMessage.Content, messageType)!;
                await publisher.Publish(domainEvent, cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Exception while processing outbox message {MessageId}", outboxMessage.Id);
                exception = ex;
            }

            await UpdateOutboxMessageAsync(connection, transaction, outboxMessage, exception);
        }

        await transaction.CommitAsync(cancellationToken);
        LogServiceCompletedProcessingOutboxMessages(outboxOptions.Value.ServiceName);
    }

    private async Task<IReadOnlyList<OutboxMessageResponse>> GetOutboxMessagesAsync(
        IDbConnection connection,
        IDbTransaction transaction
    )
    {
        string sql =
            $"""
             SELECT
                id      AS {nameof(OutboxMessageResponse.Id)},
                type    AS {nameof(OutboxMessageResponse.Type)},
                content AS {nameof(OutboxMessageResponse.Content)}
             FROM {outboxOptions.Value.SchemaName}.{OutboxConstants.TableName}
             WHERE processed_on_utc IS NULL
             ORDER BY occurred_on_utc
             LIMIT {outboxOptions.Value.BatchSize}
             FOR UPDATE SKIP LOCKED
             """;

        IEnumerable<OutboxMessageResponse> outboxMessages =
            await connection.QueryAsync<OutboxMessageResponse>(sql, transaction: transaction);

        return outboxMessages.ToList();
    }

    private async Task UpdateOutboxMessageAsync(
        IDbConnection connection,
        IDbTransaction transaction,
        OutboxMessageResponse outboxMessage,
        Exception? exception)
    {
        string sql =
            $"""
             UPDATE {outboxOptions.Value.SchemaName}.{OutboxConstants.TableName}
             SET processed_on_utc = @ProcessedOnUtc,
                 error            = @Error
             WHERE id = @Id
             """;

        await connection.ExecuteAsync(
            sql,
            new
            {
                outboxMessage.Id,
                ProcessedOnUtc = timeProvider.GetUtcNow(),
                Error = exception?.ToString()
            },
            transaction);
    }

    [LoggerMessage(LogLevel.Information, "{Service} - Beginning to process outbox messages")]
    private partial void LogServiceBeginningToProcessOutboxMessages(string service);

    [LoggerMessage(LogLevel.Information, "{Service} - Completed processing outbox messages")]
    private partial void LogServiceCompletedProcessingOutboxMessages(string service);
}
