using BuildingBlocks.Domain;

namespace BookVault.Catalog.Domain.Authors;

public sealed class Author : Entity
{
    public Author(Guid id)
        : base(id)
    {
    }
}