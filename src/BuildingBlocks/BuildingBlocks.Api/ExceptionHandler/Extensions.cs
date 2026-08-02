using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Api.ExceptionHandler;

public static class Extensions
{
    public static IServiceCollection AddCustomProblemDetails(this IServiceCollection services)
    {
        services.AddProblemDetails(options =>
        {
            options.CustomizeProblemDetails = context =>
            {
                context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);
            };
        });

        return services;
    }
}
