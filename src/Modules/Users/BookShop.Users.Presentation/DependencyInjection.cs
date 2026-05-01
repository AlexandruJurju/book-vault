using BookShop.Users.Infrastructure;
using BuildingBlocks.AspNetCore.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace BookShop.Users.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddUsersModule(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpoints(typeof(DependencyInjection).Assembly);

        builder.Services.AddInfrastructure(builder.Configuration);

        return builder.Services;
    }
}
