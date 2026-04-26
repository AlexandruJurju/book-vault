using BuildingBlocks.Infrastructure.Keycloak;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi;

namespace BuildingBlocks.AspNetCore.OpenApi.Transformers;

internal sealed class SecuritySchemeDefinitionsTransformer(IConfiguration configuration) : IOpenApiDocumentTransformer
{
    public async Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
        KeycloakOptions keycloakOptions = configuration.GetRequiredSection($"{KeycloakOptions.SectionName}").Get<KeycloakOptions>()!;

        var securityScheme = new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.OAuth2,
            Description = "OAuth2 security scheme for the Bookshop",
            Flows = new()
            {
                AuthorizationCode = new()
                {
                    Scopes = new Dictionary<string, string>()
                    {
                        { "openid", "openid" },
                        { "profile", "profile" },
                    },
                    AuthorizationUrl = KeycloakEndpoints.ToUri(keycloakOptions.BaseUrl, KeycloakEndpoints.Authorize(keycloakOptions.Realm)),
                    TokenUrl = KeycloakEndpoints.ToUri(keycloakOptions.BaseUrl, KeycloakEndpoints.Token(keycloakOptions.Realm)),
                },
            },
        };
        document.Components ??= new();
        document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();
        document.Components.SecuritySchemes.Add(OAuthDefaults.DisplayName, securityScheme);
    }
}
