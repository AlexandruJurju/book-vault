using System.Reflection;
using BookShop.Shared.Aspire;
using BuildingBlocks.Application.CQRS.Behaviors;
using BuildingBlocks.AspNetCore.OpenApi;
using BuildingBlocks.Infrastructure.Authentication;
using BuildingBlocks.Infrastructure.Authorization;
using BuildingBlocks.Infrastructure.Cache;
using BuildingBlocks.Infrastructure.Data;
using BuildingBlocks.Infrastructure.EntityFramework.Interceptors;
using FluentValidation;
using Mediator;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TickerQ.Dashboard.DependencyInjection;
using TickerQ.DependencyInjection;

namespace BookShop.WebApi;

internal static class DependencyInjection
{
    public static void AddPresentation(this IServiceCollection services)
    {
        services.AddCustomOpenApi();
    }

    public static void AddApplication(this IServiceCollection services, Assembly[] moduleAssemblies)
    {
        services.AddMediator(options =>
        {
            options.ServiceLifetime = ServiceLifetime.Scoped;
        });

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

        services.AddValidatorsFromAssemblies(moduleAssemblies, includeInternalTypes: true);
    }

    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCustomAuthentication();

        services.AddCustomAuthorization();

        services.TryAddSingleton(TimeProvider.System);

        services.AddCustomCache(configuration);

        services.AddCustomNpgsql(configuration, Resources.Postgres);
        services.TryAddSingleton<InsertOutboxMessagesInterceptor>();

        services.AddTickerQ(opt =>
        {
            opt.AddDashboard(dashboard =>
            {
                dashboard.SetBasePath("/management/jobs");
            });
        });
    }
}
