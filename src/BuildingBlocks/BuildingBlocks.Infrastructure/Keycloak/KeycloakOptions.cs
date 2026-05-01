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
    public string PublicClientId { get; init; }

    public string ConfidentialClientId { get; init; }

    public string ConfidentialClientSecret { get; init; }

    private string Base => BaseUrl.TrimEnd('/');

    public string Issuer
        => $"{Base}/realms/{Realm}";

    public string AdminUrl
        => $"{Base}/admin/realms/{Realm}/";

    public string TokenUrl
        => $"{Base}/realms/{Realm}/protocol/openid-connect/token";

    public string AuthorizeUrl
        => $"{Base}/realms/{Realm}/protocol/openid-connect/auth";

    public string IntrospectUrl
        => $"{Base}/realms/{Realm}/protocol/openid-connect/token/introspect";

    public string LogoutUrl
        => $"{Base}/realms/{Realm}/protocol/openid-connect/logout";

    public string MetadataUrl
        => $"{Base}/realms/{Realm}/.well-known/openid-configuration";
}
