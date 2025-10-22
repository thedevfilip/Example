using Microsoft.AspNetCore.Mvc;

namespace Example.Api.Features.Organizations.Registration;

internal static class CreateOrganizationEndpoint
{
    public static IEndpointRouteBuilder MapRegisterOrganization(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/organization", async (
            [FromServices] RegisterOrganizationHandler handler,
            [FromBody] RegisterOrganizationRequest request) =>
        {
            RegisterOrganizationResponse response = await handler.HandleAsync(request);

            return Results.Ok(response);
        });

        return endpoints;
    }
}
