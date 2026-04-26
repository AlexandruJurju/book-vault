using BuildingBlocks.Infrastructure.Keycloak;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace BuildingBlocks.Infrastructure.Authentication;

public static class WebApplicationBuilderExtensions
{
    public static IServiceCollection AddCustomKeycloakAuthentication(
        this WebApplicationBuilder builder
    )
    {
        IServiceCollection services = builder.Services;
        ConfigurationManager configuration = builder.Configuration;

        services
            .AddOptionsWithValidateOnStart<KeycloakOptions>()
            .Bind(configuration.GetSection(KeycloakOptions.SectionName))
            .ValidateDataAnnotations();

        KeycloakOptions keycloakOptions = configuration
            .GetRequiredSection(KeycloakOptions.SectionName)
            .Get<KeycloakOptions>()!;

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = !builder.Environment.IsDevelopment();
                options.Audience = keycloakOptions.Audience;
                options.MetadataAddress = keycloakOptions.BaseUrl.TrimEnd('/') + KeycloakEndpoints.Metadata(keycloakOptions.Realm);
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = keycloakOptions.Issuer
                };
            });

        services.AddHttpContextAccessor();

        return services;
    }
}
