using BuildingBlocks.Domain;

namespace BookVault.Catalog.Domain.Genres;

public sealed class Genre : Entity
{
    public Genre(Guid id)
        : base(id)
    {
    }
}