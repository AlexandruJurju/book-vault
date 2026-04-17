using BookShop.Catalog.Infrastructure.Database;
using BookShop.Shared.Aspire;
using BuildingBlocks.Application.Data;
using BuildingBlocks.Infrastructure.Configuration;
using BuildingBlocks.Infrastructure.Data;
using BuildingBlocks.Infrastructure.EF;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;

namespace BookShop.Catalog.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCustomPostgresDbContext<CatalogDbContext>(configuration, CatalogResources.Database, Schemas.Catalog);

        string databaseConnectionString = configuration.GetConnectionStringOrThrow(CatalogResources.Database);
        NpgsqlDataSource npgsqlDataSource = new NpgsqlDataSourceBuilder(databaseConnectionString).Build();
        services.TryAddSingleton(npgsqlDataSource);
        services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();

        services.AddSingleton(TimeProvider.System);

        return services;
    }
}
