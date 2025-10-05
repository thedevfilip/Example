using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Example.Api.Features.Organizations.Registration;

internal static class CreateOrganizationEndpoint
{
    public static IEndpointRouteBuilder MapCreateOrganization(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/organizations", [AllowAnonymous] async (
            [FromServices] RegisterOrganizationHandler handler,
            [FromBody] RegisterOrganizationRequest request) =>
        {
            RegisterOrganizationResponse response = await handler.HandleAsync(request);
            return Results.Ok(response);
        });

        return endpoints;
    }
}
