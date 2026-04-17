using BookShop.ServiceDefaults;
using BookShop.WebApi;
using BookShop.WebApi.Extensions;
using BuildingBlocks.AspNetCore.Endpoints;
using BuildingBlocks.AspNetCore.ExceptionHandler;
using Scalar.AspNetCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Configuration.AddModuleConfiguration(["catalog", "basket", "users"]);

builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddOpenApi();

builder.Services.AddModuleMediator();

builder.AddModules();

WebApplication app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
    app.ApplyMigrations();
}

app.UseHttpsRedirection();

app.UseExceptionHandler();

app.MapEndpoints();

await app.RunAsync();
