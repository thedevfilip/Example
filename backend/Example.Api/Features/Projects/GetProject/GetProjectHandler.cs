using Example.Domain.Entities;
using Example.Domain.Primitives;
using Example.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Example.Api.Features.Projects.GetProject;

internal sealed class GetProjectHandler(AppDbContext context)
{
    public async Task<Result<GetProjectResponse>> HandleAsync(Guid projectId)
    {
        Project? project = await context.Set<Project>()
            .FirstOrDefaultAsync(p => p.Id == projectId);

        if (project is null)
        {
            return GetProjectErrors.ProjectNotFound;
        }

        return new GetProjectResponse(
            project.Id,
            project.Name,
            project.Description,
            project.CreatedAt);
    }
}
