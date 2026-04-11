using Projects;
using Scalar.Aspire;

IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

IResourceBuilder<PostgresServerResource> postgres = builder
    .AddPostgres("postgres")
    .WithLifetime(ContainerLifetime.Persistent)
    .WithDataVolume();

IResourceBuilder<PostgresDatabaseResource> catalogDb = postgres.AddDatabase("catalogDb");

IResourceBuilder<ProjectResource> catalogApi = builder
    .AddProject<BookShop_Catalog_Api>("bookshop-catalog-api")
    .WithReference(catalogDb)
    .WaitFor(catalogDb);

builder.AddScalarApiReference()
    .WithApiReference(catalogApi);

await builder.Build().RunAsync();
