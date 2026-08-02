using BuildingBlocks.Api.OpenApi.Transformers;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Api.OpenApi;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCustomOpenApi(this IServiceCollection services)
    {
        services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer<SecuritySchemeDefinitionsTransformer>();
            options.AddDocumentTransformer<SecurityRequirementsDocumentTransformer>();
        });

        return services;
    }
}
