using BookVault.Catalog.Application;
using BookVault.ServiceDefaults;
using BuildingBlocks.Chassis.Endpoints;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddOpenApi();
builder.Services.AddApplication();

builder.Services.AddEndpoints(typeof(Program).Assembly);

var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment()) { }

app.UseHttpsRedirection();

app.MapEndpoints();

app.UseDefaultOpenApi();

app.Run();
