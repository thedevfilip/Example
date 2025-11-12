using Example.Domain.Primitives;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Example.Api.Features.Projects.GetProject;

internal static class GetProjectEndpoint
{
    public static IEndpointRouteBuilder MapGetProject(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/projects/{id:guid}", [Authorize] async (
            [FromServices] GetProjectHandler handler,
            Guid id) =>
        {
            Result<GetProjectResponse> result = await handler.HandleAsync(id);

            return result.Match(
                Results.Ok,
                error => Results.NotFound(error));
        });

        return endpoints;
    }
}
