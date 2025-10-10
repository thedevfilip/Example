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
            RefreshTokenResponse? response = await handler.HandleAsync(request);

            return response is not null
                ? Results.Ok(response)
                : Results.BadRequest("Invalid token.");
        });

        return endpoints;
    }
}
