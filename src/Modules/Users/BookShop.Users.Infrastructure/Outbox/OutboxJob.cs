using System.Collections.Concurrent;
using System.Data;
using System.Data.Common;
using System.Text.Json;
using BookShop.Shared.Aspire;
using BuildingBlocks.Application.Data;
using BuildingBlocks.Infrastructure.Outbox;
using Dapper;
using Mediator;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TickerQ.Exceptions;
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
    private static string ServiceName => Services.Users;
    private static string SchemaName => Services.Users;

    public async Task ExecuteAsync(TickerFunctionContext context, CancellationToken cancellationToken = default)
    {
        LogServiceBeginningToProcessOutboxMessages(ServiceName);

        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();
        await using DbTransaction transaction = await connection.BeginTransactionAsync(cancellationToken);

        IReadOnlyList<OutboxMessageResponse> outboxMessages = await GetOutboxMessagesAsync(connection, transaction);

        if (outboxMessages.Count == 0)
        {
            throw new TerminateExecutionException("No outbox messages to process");
        }

        var updateQueue = new ConcurrentQueue<OutboxUpdate>();
        var typeCache = new ConcurrentDictionary<string, Type>();

        await PublishMessagesAsync(outboxMessages, typeCache, updateQueue, cancellationToken);

        await UpdateOutboxMessagesAsync(connection, transaction, updateQueue);

        await transaction.CommitAsync(cancellationToken);

        LogServiceCompletedProcessingOutboxMessages(ServiceName);
    }

    private async Task PublishMessagesAsync(
        IReadOnlyList<OutboxMessageResponse> outboxMessages,
        ConcurrentDictionary<string, Type> typeCache,
        ConcurrentQueue<OutboxUpdate> updateQueue,
        CancellationToken cancellationToken
    )
    {
        foreach (OutboxMessageResponse outboxMessage in outboxMessages)
        {
            Exception? exception = null;
            try
            {
                Type messageType = GetOrAddMessageType(typeCache, outboxMessage.Type);
                object domainEvent = JsonSerializer.Deserialize(outboxMessage.Content, messageType)!;
                await publisher.Publish(domainEvent, cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Exception while processing outbox message {MessageId}", outboxMessage.Id);
                exception = ex;
            }

            updateQueue.Enqueue(new OutboxUpdate(outboxMessage.Id, timeProvider.GetUtcNow().UtcDateTime, exception?.ToString()));
        }
    }

    private static async Task UpdateOutboxMessagesAsync(
        IDbConnection connection,
        IDbTransaction transaction,
        ConcurrentQueue<OutboxUpdate> updateQueue)
    {
        if (updateQueue.IsEmpty)
        {
            return;
        }

        string updateSql =
            $$"""
                 UPDATE {{SchemaName}}.{{OutboxConstants.TableName}}
                 SET processed_on_utc = v.processed_on_utc,
                     error = v.error
                 FROM UNNEST(@Ids, @ProcessedAts, @Errors)
                    AS v(id, processed_on_utc, error)
                 WHERE {{SchemaName}}.{{OutboxConstants.TableName}}.id = v.id::uuid
              """;

        var parameters = new
        {
            Ids = updateQueue.Select(x => x.Id).ToArray(),
            ProcessedAts = updateQueue.Select(x => x.ProcessedOnUtc).ToArray(),
            Errors = updateQueue.Select(x => x.Exception).ToArray()
        };

        await connection.ExecuteAsync(updateSql, parameters, transaction: transaction);
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
             FROM {SchemaName}.{OutboxConstants.TableName}
             WHERE processed_on_utc IS NULL
             ORDER BY occurred_on_utc
             LIMIT {outboxOptions.Value.BatchSize}
             FOR UPDATE SKIP LOCKED
             """;

        IEnumerable<OutboxMessageResponse> outboxMessages = await connection.QueryAsync<OutboxMessageResponse>(sql, transaction: transaction);

        return outboxMessages.ToList();
    }


    private static Type GetOrAddMessageType(ConcurrentDictionary<string, Type> typeCache, string typeName)
    {
        return typeCache.GetOrAdd(typeName, name => Domain.AssemblyReference.Assembly.GetType(name)!);
    }

    [LoggerMessage(LogLevel.Information, "{Service} - Beginning to process outbox messages")]
    private partial void LogServiceBeginningToProcessOutboxMessages(string service);

    [LoggerMessage(LogLevel.Information, "{Service} - Completed processing outbox messages")]
    private partial void LogServiceCompletedProcessingOutboxMessages(string service);
}
