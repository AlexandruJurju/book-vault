using BuildingBlocks.Chassis.Endpoints;

namespace BookVault.Catalog.Api.Endpoints.Authors;

internal sealed class ListAuthorsEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("test", async () => { });
    }
}