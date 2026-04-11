using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BuildingBlocks.Chassis.EF;

public static class DbContextExtensions
{
    public static void AddPostgresDbContext<TDbContext>(
        this IHostApplicationBuilder builder,
        string                       connectionName,
        Action<IServiceCollection>?  action = null
    ) where TDbContext : DbContext
    {
        IServiceCollection services = builder.Services;

        builder.AddNpgsqlDbContext<TDbContext>(
            connectionName,
            configureDbContextOptions: options => options.UseSnakeCaseNamingConvention());


        action?.Invoke(services);
    }
}
