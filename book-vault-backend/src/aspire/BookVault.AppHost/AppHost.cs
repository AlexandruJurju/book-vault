var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.BookVault_Catalog_Api>("bookvault-catalog-api");

builder.Build().Run();