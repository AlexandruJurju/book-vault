using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Infrastructure.Authorization;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCustomAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization();
        
        return services;
    }
}
