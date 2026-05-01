using BookShop.Shared.Aspire;
using BookShop.Users.Application.Abstractions;
using BookShop.Users.Domain.Users;
using BookShop.Users.Infrastructure.EntityFramework;
using BookShop.Users.Infrastructure.Outbox;
using BookShop.Users.Infrastructure.Users;
using BuildingBlocks.Infrastructure.Authentication;
using BuildingBlocks.Infrastructure.Cache;
using BuildingBlocks.Infrastructure.Data;
using BuildingBlocks.Infrastructure.EntityFramework;
using BuildingBlocks.Infrastructure.EntityFramework.Interceptors;
using BuildingBlocks.Infrastructure.Time;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TickerQ.Dashboard.DependencyInjection;
using TickerQ.DependencyInjection;

namespace BookShop.Users.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this WebApplicationBuilder builder
    )
    {
        IServiceCollection services = builder.Services;
        ConfigurationManager configuration = builder.Configuration;

        services.AddTimeProvider();

        services.AddCustomMemoryCache(configuration, Services.Users);

        services.AddCustomPostgresDbContext<UsersDbContext>(configuration, Resources.Postgres, Services.Users);
        services.AddScoped<IUsersDbContext>(provider => provider.GetRequiredService<UsersDbContext>());
        services.AddCustomNpgsql(configuration, Resources.Postgres);
        services.TryAddScoped<IInterceptor, InsertDomainEventsInterceptor>();

        AddOutboxProcessor(services, configuration);

        builder.AddCustomKeycloakAuthentication();

        return services;
    }

    private static void AddOutboxProcessor(IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddTickerQ(opt =>
        {
            opt.AddDashboard(dashboard =>
            {
                dashboard.SetBasePath("/management/jobs");
            });
        });

        IConfigurationSection section = configuration
            .GetRequiredSection($"{Services.Users}:{OutboxJobOptions.ConfigurationSection}");

        services
            .AddOptionsWithValidateOnStart<OutboxJobOptions>()
            .Bind(section)
            .ValidateDataAnnotations();

        services.AddScoped<OutboxProcessor>();
        services.AddScoped<OutboxJob>();

        OutboxJobOptions outboxJobOptions = section.Get<OutboxJobOptions>()!;

        services
            .MapTicker<OutboxJob>()
            .WithMaxConcurrency(1)
            .WithCron(outboxJobOptions.Cron);
    }
}
