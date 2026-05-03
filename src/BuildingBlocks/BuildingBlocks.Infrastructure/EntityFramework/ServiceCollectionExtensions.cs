using BuildingBlocks.Application.Data;
using BuildingBlocks.Infrastructure.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Infrastructure.EntityFramework;

public static class ServiceCollectionExtensions
{
    public static void AddCustomPostgresDbContext<TDbContext>(
        this IServiceCollection services,
        IConfiguration configuration,
        string postgresResourceName,
        string service
    ) where TDbContext : DbContext
    {
        string connectionString = configuration.GetConnectionStringOrThrow(postgresResourceName);

        services.AddDbContext<TDbContext>((sp, options) =>
            {
                options
                    .UseNpgsql(
                        connectionString,
                        npgsqlOptions =>
                            npgsqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName, service)
                    )
                    .UseSnakeCaseNamingConvention();

                IInterceptor[] interceptors = sp.GetServices<IInterceptor>()
                    .ToArray();

                if (interceptors.Length != 0)
                {
                    options.AddInterceptors(interceptors);
                }
            }
        );
    }

    public static void AddCustomPostgresDbContext<TDbContext>(
        this IServiceCollection services,
        IConfiguration configuration,
        string postgresResourceName
    ) where TDbContext : DbContext
    {
        string connectionString = configuration.GetConnectionStringOrThrow(postgresResourceName);

        services.AddDbContext<TDbContext>((sp, options) =>
            {
                options
                    .UseNpgsql(
                        connectionString,
                        npgsqlOptions =>
                            npgsqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName)
                    )
                    .UseSnakeCaseNamingConvention();

                IInterceptor[] interceptors = sp.GetServices<IInterceptor>()
                    .ToArray();

                if (interceptors.Length != 0)
                {
                    options.AddInterceptors(interceptors);
                }
            }
        );
    }
}
