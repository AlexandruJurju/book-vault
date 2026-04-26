namespace BuildingBlocks.Infrastructure.Keycloak;

public static class KeycloakEndpoints
{
    public static string Token(string realm)
        => $"/realms/{realm}/protocol/openid-connect/token";

    public static string Authorize(string realm)
        => $"/realms/{realm}/protocol/openid-connect/auth";

    public static string Introspect(string realm)
        => $"/realms/{realm}/protocol/openid-connect/token/introspect";

    public static string Logout(string realm)
        => $"/realms/{realm}/protocol/openid-connect/logout";

    public static string Metadata(string realm)
        => $"/realms/{realm}/.well-known/openid-configuration";

    public static Uri ToUri(string baseUrl, string realmPath)
        => new(baseUrl.TrimEnd('/') + realmPath);

    public static Uri ToUri(Uri baseUrl, string realmPath)
        => new(baseUrl, realmPath);
}
