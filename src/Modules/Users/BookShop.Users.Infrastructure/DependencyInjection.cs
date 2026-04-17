using BookShop.Shared.Aspire;
using BookShop.Users.Infrastructure.EntityFramework;
using BuildingBlocks.Infrastructure.Cache;
using BuildingBlocks.Infrastructure.EF;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BookShop.Users.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCustomPostgresDbContext<UsersDbContext>(configuration, UsersResources.Database, Schemas.Users);

        services.AddCustomMemoryCache(configuration);

        return services;
    }
}
