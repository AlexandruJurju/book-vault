using BuildingBlocks.AspNetCore.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace BookShop.Users.Api.Endpoints.Users;

internal sealed class RegisterUserEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("users", Handle)
            .WithTags(Tags.Users)
            .AllowAnonymous();
    }

    private static async Task<IResult> Handle()
    {
        return Results.Ok();
    }
}
