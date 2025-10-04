using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Example.Api.Features.Users.Info;

internal static class UserInfoEndpoint
{
    public static IEndpointRouteBuilder MapUserInfo(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/users/info", [Authorize] async (
            ClaimsPrincipal user,
            [FromServices] UserInfoHandler handler) =>
        {
            UserInfoResponse? result = await handler.HandleAsync(user);
            return Results.Ok(result);
        });

        return endpoints;
    }
}
