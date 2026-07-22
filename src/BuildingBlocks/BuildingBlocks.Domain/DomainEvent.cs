using BuildingBlocks.Common.Helpers;

namespace BuildingBlocks.Domain;

public abstract class DomainEvent : IDomainEvent
{
    protected DomainEvent()
    {
        Id = GuidProvider.NewGuid();
        OccurredAtUtc = DateTime.UtcNow;
    }

    protected DomainEvent(Guid id, DateTime occurredOnUtc)
    {
        Id = id;
        OccurredAtUtc = occurredOnUtc;
    }

    public Guid Id { get; init; }

    public DateTime OccurredAtUtc { get; init; }
}
