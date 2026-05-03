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
    .AddKeycloak(Resources.Keycloak, 8080, keycloakAdminUsername, keycloakAdminPassword)
    .WithLifetime(ContainerLifetime.Persistent)
    .WithDataVolume();

IResourceBuilder<RedisResource> redis = builder
    .AddRedis(Resources.Redis, 6379)
    .WithLifetime(ContainerLifetime.Persistent)
    .WithDataVolume();

IResourceBuilder<ParameterResource> rabbitMqUser = builder.AddParameter("rabbitmq-user", "rabbitmq", secret: false);
IResourceBuilder<ParameterResource> rabbitMqPassword = builder.AddParameter("rabbitmq-password", "rabbitmq", secret: true);
IResourceBuilder<RabbitMQServerResource> rabbitMq = builder
    .AddRabbitMQ(Resources.RabbitMq, userName: rabbitMqUser, password: rabbitMqPassword, port: 5672)
    .WithLifetime(ContainerLifetime.Persistent)
    .WithDataVolume()
    .WithManagementPlugin();

builder.AddProject<BookShop_WebApi>("bookshop-webapi")
    .WithReference(postgres).WaitFor(postgres)
    .WithReference(keycloak).WaitFor(keycloak)
    .WithReference(redis).WaitFor(redis)
    .WithReference(rabbitMq).WaitFor(rabbitMq)
    ;

await builder.Build().RunAsync();
