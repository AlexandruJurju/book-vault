using BookVault.Catalog.Domain.Authors;
using BookVault.Catalog.Domain.Books;
using BookVault.Catalog.Domain.Genres;
using BookVault.Catalog.Domain.Publishers;
using BuildingBlocks.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace BookVault.Catalog.Infrastructure;

public sealed class CatalogDbContext(DbContextOptions<CatalogDbContext> options) :
    DbContext(options), IUnitOfWork
{
    public DbSet<Author> Authors => Set<Author>();
    public DbSet<Book> Books => Set<Book>();
    public DbSet<Genre> Genres => Set<Genre>();
    public DbSet<Publisher> Publishers => Set<Publisher>();
}