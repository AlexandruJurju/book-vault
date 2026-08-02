using BookShop.Catalog.Infrastructure;
using BuildingBlocks.Api.Endpoints;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BookShop.Catalog.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddCatalogModule(this IHostApplicationBuilder builder)
    {
        builder.Services.AddPresentation();

        builder.AddInfrastructure();

        return builder.Services;
    }

    private static void AddPresentation(this IServiceCollection services)
    {
        services.AddEndpoints(typeof(DependencyInjection).Assembly);
    }
}
