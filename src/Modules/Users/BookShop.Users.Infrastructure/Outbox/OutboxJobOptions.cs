using System.ComponentModel.DataAnnotations;

namespace BookShop.Users.Infrastructure.Outbox;

public sealed class OutboxJobOptions
{
    public const string ConfigurationSection = "OutboxJob";

    [Required]
    public string SchemaName { get; init; }

    [Required]
    public string ServiceName { get; init; }

    [Required]
    public string Cron { get; init; }

    [Required]
    [Range(1, int.MaxValue)]
    public int BatchSize { get; init; }
}
