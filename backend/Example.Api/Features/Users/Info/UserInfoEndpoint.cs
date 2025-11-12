using Example.Domain.Primitives;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Example.Api.Features.Users.Info;

internal static class UserInfoEndpoint
{
    public static IEndpointRouteBuilder MapUserInfo(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/users/info", async (
            ClaimsPrincipal user,
            [FromServices] UserInfoHandler handler) =>
        {
            Result<UserInfoResponse> result = await handler.HandleAsync(user);
            return result.Match(Results.Ok, error => Results.BadRequest(error));
        });

        return endpoints;
    }
}
