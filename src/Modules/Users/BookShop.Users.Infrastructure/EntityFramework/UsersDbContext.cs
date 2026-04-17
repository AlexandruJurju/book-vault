using BookShop.Users.Domain.Users;
using BuildingBlocks.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Users.Infrastructure.EntityFramework;

public sealed class UsersDbContext(
    DbContextOptions<UsersDbContext> options
) : DbContext(options), IUnitOfWork
{
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UsersDbContext).Assembly);
    }
}
