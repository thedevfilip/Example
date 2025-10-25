using Example.Domain.Primitives;
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
            Result<RegisterUserResponse> result = await handler.HandleAsync(request);

            return result.Match(Results.Ok, error => Results.BadRequest(error.Desription));
        }).RequireRateLimiting("register");

        return endpoints;
    }
}
