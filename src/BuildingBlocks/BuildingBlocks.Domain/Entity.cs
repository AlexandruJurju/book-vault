namespace BuildingBlocks.Domain;

public abstract class Entity
{
    protected Entity(Guid id)
    {
        Id = id;
    }

    protected Entity()
    {
    }

    public Guid Id { get; init; }

    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void ClearDomainEvents()
        => _domainEvents.Clear();

    public void RaiseDomainEvent(IDomainEvent domainEvent)
        => _domainEvents.Add(domainEvent);
}
