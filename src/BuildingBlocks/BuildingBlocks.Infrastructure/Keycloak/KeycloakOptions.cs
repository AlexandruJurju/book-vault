using System.ComponentModel.DataAnnotations;

namespace BuildingBlocks.Infrastructure.Keycloak;

public sealed class KeycloakOptions
{
    public const string SectionName = "Keycloak";

    [Required]
    public string BaseUrl { get; init; }

    [Required]
    public string Realm { get; init; }

    [Required]
    public string ClientId { get; init; }

    [Required]
    public string Audience { get; init; }

    public string Issuer
        => $"{BaseUrl.TrimEnd('/')}/realms/{Realm}";
}
