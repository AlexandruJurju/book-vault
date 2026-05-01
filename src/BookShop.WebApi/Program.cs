using System.Reflection;
using BookShop.Catalog;
using BookShop.ServiceDefaults;
using BookShop.Users.Presentation;
using BookShop.WebApi;
using BookShop.WebApi.Extensions;
using BuildingBlocks.AspNetCore.Endpoints;
using BuildingBlocks.AspNetCore.ExceptionHandler;
using BuildingBlocks.AspNetCore.Scalar;
using TickerQ.DependencyInjection;
using AssemblyReference = BookShop.Users.Application.AssemblyReference;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

Assembly[] moduleApplicationAssemblies =
[
    AssemblyReference.Assembly,
    BookShop.Basket.Application.AssemblyReference.Assembly,
    BookShop.Catalog.AssemblyReference.Assembly
];

builder.Services.AddPresentation();
builder.Services.AddApplication(moduleApplicationAssemblies);
builder.Services.AddInfrastructure(builder.Configuration);

builder.AddCatalogModule();
builder.AddUsersModule();

WebApplication app = builder.Build();

app.MapDefaultEndpoints();

app.MapCustomScalar(builder.Configuration);

if (app.Environment.IsDevelopment())
{
    app.ApplyMigrations();
}

app.UseTickerQ();

app.UseHttpsRedirection();

app.UseExceptionHandler();

app.UseAuthentication();

app.UseAuthorization();

app.MapEndpoints();

await app.RunAsync();
