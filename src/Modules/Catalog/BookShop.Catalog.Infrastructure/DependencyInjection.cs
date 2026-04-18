using BookShop.Catalog.Infrastructure.EntityFramework;
using BookShop.Shared.Aspire;
using BuildingBlocks.Infrastructure.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BookShop.Catalog.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCustomPostgresDbContext<CatalogDbContext>(configuration, Resources.Postgres, Services.Catalog);

        services.AddSingleton(TimeProvider.System);

        return services;
    }
}
