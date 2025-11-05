using Example.Domain.Primitives;
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
            Result<RegisterOrganizationResponse> result = await handler.HandleAsync(request);

            return result.Match(Results.Ok, error => Results.BadRequest(error));
        });

        return endpoints;
    }
}
