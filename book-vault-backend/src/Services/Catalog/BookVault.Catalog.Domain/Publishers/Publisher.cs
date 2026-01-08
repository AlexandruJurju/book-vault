using BuildingBlocks.Domain;

namespace BookVault.Catalog.Domain.Publishers;

public sealed class Publisher : Entity
{
    public Publisher(Guid id)
        : base(id)
    {
    }
}