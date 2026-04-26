namespace BuildingBlocks.Infrastructure.Outbox;

public record struct OutboxUpdate(
    Guid Id,
    DateTime ProcessedOnUtc,
    string? Exception
);
