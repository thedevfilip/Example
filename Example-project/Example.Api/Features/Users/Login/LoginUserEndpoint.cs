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
            LoginUserResponse? response = await handler.HandleAsync(request);
            return response is not null
                ? Results.Ok(response)
                : Results.BadRequest("Invalid credentials.");
        });

        return endpoints;
    }
}
