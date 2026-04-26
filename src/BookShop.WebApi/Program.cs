using BookShop.ServiceDefaults;
using BookShop.WebApi;
using BookShop.WebApi.Extensions;
using BuildingBlocks.AspNetCore.Endpoints;
using BuildingBlocks.AspNetCore.ExceptionHandler;
using BuildingBlocks.AspNetCore.OpenApi;
using BuildingBlocks.AspNetCore.Scalar;
using Scalar.AspNetCore;
using TickerQ.DependencyInjection;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddCustomOpenApi();

builder.Services.AddModuleMediator();

builder.AddModules();

WebApplication app = builder.Build();

app.MapPost("something", () =>
{
    return Results.Ok();
}).RequireAuthorization();

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
