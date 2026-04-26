using BookShop.Shared.Aspire;
using Projects;

IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

IResourceBuilder<ParameterResource> pgUser = builder.AddParameter("postgres-user", "app_user", secret: false);
IResourceBuilder<ParameterResource> pgPassword = builder.AddParameter("postgres-password", "super-secret-password", secret: true);
IResourceBuilder<PostgresServerResource> postgres = builder
    .AddPostgres(Resources.Postgres, port: 5432)
    .WithLifetime(ContainerLifetime.Persistent)
    .WithDataVolume()
    .WithUserName(pgUser)
    .WithPassword(pgPassword);

IResourceBuilder<ParameterResource> keycloakAdminUsername = builder.AddParameter("keycloak-user", "admin", secret: false);
IResourceBuilder<ParameterResource> keycloakAdminPassword = builder.AddParameter("keycloak-password", "admin", secret: true);
IResourceBuilder<KeycloakResource> keycloak = builder
    .AddKeycloak(Resources.Keycloak, port: 8080, adminUsername: keycloakAdminUsername, adminPassword: keycloakAdminPassword)
    .WithLifetime(ContainerLifetime.Persistent)
    .WithDataVolume();

IResourceBuilder<RedisResource> redis = builder
    .AddRedis(Resources.Redis, port: 6379)
    .WithLifetime(ContainerLifetime.Persistent)
    .WithDataVolume();

builder.AddProject<BookShop_WebApi>("bookshop-webapi")
    .WithReference(postgres).WaitFor(postgres)
    .WithReference(keycloak).WaitFor(keycloak)
    .WithReference(redis).WaitFor(redis)
    ;

await builder.Build().RunAsync();
