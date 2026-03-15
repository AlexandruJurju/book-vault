using Scalar.Aspire;

var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder
    .AddPostgres("postgres")
    .WithLifetime(ContainerLifetime.Persistent)
    .WithDataVolume();

var catalogDb = postgres.AddDatabase("catalogDb");

var catalogApi = builder
    .AddProject<Projects.BookVault_Catalog_Api>("bookvault-catalog-api")
    .WithReference(catalogDb)
    .WaitFor(catalogDb);

builder.AddScalarApiReference()
    .WithApiReference(catalogApi);

builder.Build().Run();