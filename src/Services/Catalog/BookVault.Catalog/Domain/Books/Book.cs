using BookVault.Catalog.Domain.Shared;
using BuildingBlocks.Domain;

namespace BookVault.Catalog.Domain.Books;

public sealed class Book : Entity
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public Money Price { get; private set; }

    private Book(Guid id,
        string name,
        string description,
        Money price)
        : base(id)
    {
        Name = name;
        Description = description;
        Price = price;
    }

    public static Book New(string name, string description, Money price)
    {
        return new Book(Guid.NewGuid(), name, description, price);
    }
}