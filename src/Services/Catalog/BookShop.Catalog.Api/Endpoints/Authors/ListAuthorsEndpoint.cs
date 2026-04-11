using BuildingBlocks.Chassis.Endpoints;

namespace BookShop.Catalog.Api.Endpoints.Authors;

internal sealed class ListAuthorsEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app) => app.MapPost("test", Handle);

    private static async Task<IResult> Handle() => Results.Ok();
}
