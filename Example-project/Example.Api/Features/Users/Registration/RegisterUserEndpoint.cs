using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Example.Api.Features.Users.Registration;

internal static class RegisterUserEndpoint
{
    public static IEndpointRouteBuilder MapRegisterUser(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/users/register", [AllowAnonymous] async (
            [FromServices] RegisterUserHandler handler,
            [FromBody] RegisterUserRequest request) =>
        {
            RegisterUserResponse? response = await handler.HandleAsync(request);

            return response is not null
                ? Results.Ok(response)
                : Results.BadRequest("User registration failed.");
        });

        return endpoints;
    }
}
