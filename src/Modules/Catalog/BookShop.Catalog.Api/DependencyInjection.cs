using BookShop.Catalog.Application;
using BookShop.Catalog.Infrastructure;
using BuildingBlocks.AspNetCore.Endpoints;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BookShop.Catalog.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddCatalogModule(this IHostApplicationBuilder builder)
    {
        builder.Services.AddEndpoints(typeof(DependencyInjection).Assembly);

        builder.Services.AddApplication();

        builder.Services.AddInfrastructure(builder.Configuration);

        return builder.Services;
    }
}
