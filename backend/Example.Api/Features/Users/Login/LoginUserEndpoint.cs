using Example.Domain.Primitives;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Example.Api.Features.Users.Login;

internal static class LoginUserEndpoint
{
    public static IEndpointRouteBuilder MapLoginUser(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/users/login", [AllowAnonymous] async (
            [FromServices] LoginUserHandler handler,
            [FromBody] LoginUserRequest request) =>
        {
            Result<LoginUserResponse> result = await handler.HandleAsync(request);

            return result.Match(Results.Ok, error => Results.BadRequest(error));
        }).RequireRateLimiting("login-refresh");

        return endpoints;
    }
}
