using Example.Domain.Primitives;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Example.Api.Features.Projects.CreateProject;

internal static class CreateProjectEndpoint
{
    public static IEndpointRouteBuilder MapCreateProject(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/projects", [Authorize] async (
            [FromServices] CreateProjectHandler handler,
            [FromBody] CreateProjectRequest request) =>
        {
            Result<CreateProjectResponse> result = await handler.HandleAsync(request);

            return result.Match(
                success => Results.Created($"/api/projects/{success.Id}", success),
                error => Results.BadRequest(error));
        });

        return endpoints;
    }
}
