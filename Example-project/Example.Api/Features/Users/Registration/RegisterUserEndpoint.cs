using Microsoft.AspNetCore.Mvc;

namespace Example.Api.Features.Users.Registration;

public static class RegisterUserEndpoint
{
    public static IEndpointRouteBuilder MapRegisterUser(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/users/register", async (
            [FromServices] RegisterUserHandler handler,
            [FromBody] RegisterUserRequest request) =>
        {
            var response = await handler.HandleAsync(request);
            
            return response is not null
                ? Results.Ok(response)
                : Results.BadRequest("User registration failed.");
        });

        return endpoints;
    }
}