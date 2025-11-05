using Example.Domain.Primitives;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Example.Api.Features.Users.RefreshTokenLogin;

internal static class RefreshTokenEndpoint
{
    public static IEndpointRouteBuilder MapRefreshToken(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/users/refresh-token", [AllowAnonymous] async (
            [FromServices] RefreshTokenHandler handler,
            [FromBody] RefreshTokenRequest request) =>
        {
            Result<RefreshTokenResponse> result = await handler.HandleAsync(request);

            return result.Match(Results.Ok, error => Results.BadRequest(error));
        }).RequireRateLimiting("login-refresh");

        return endpoints;
    }
}
