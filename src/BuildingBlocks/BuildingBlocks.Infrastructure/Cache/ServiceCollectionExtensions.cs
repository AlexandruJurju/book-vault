using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Infrastructure.Cache;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCustomCache(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services
            .AddOptionsWithValidateOnStart<CacheOptions>()
            .Bind(configuration.GetRequiredSection(CacheOptions.ConfigurationSection))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        CacheOptions cacheOptions = configuration
            .GetRequiredSection(CacheOptions.ConfigurationSection)
            .Get<CacheOptions>()!;

        services.AddHybridCache(options =>
        {
            options.MaximumPayloadBytes = cacheOptions.MaximumPayloadBytes;

            options.DefaultEntryOptions = new HybridCacheEntryOptions
            {
                LocalCacheExpiration = TimeSpan.FromMinutes(cacheOptions.LocalExpirationInMinutes),
                Expiration = TimeSpan.FromMinutes(cacheOptions.DistributedExpirationInMinutes)
            };
        });

        return services;
    }
}
