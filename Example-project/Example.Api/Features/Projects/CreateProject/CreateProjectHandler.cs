using Example.Domain.Contexts;
using Example.Domain.Entities;
using Example.Domain.Primitives;
using Example.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Example.Api.Features.Projects.CreateProject;

internal sealed class CreateProjectHandler(
    AppDbContext context,
    IOrganizationContext organizationContext,
    IHttpContextAccessor httpContextAccessor)
{
    public async Task<Result<CreateProjectResponse>> HandleAsync(CreateProjectRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name) || request.Name.Length > 200)
        {
            return CreateProjectErrors.InvalidProjectName;
        }

        Guid organizationId = organizationContext.OrganizationId;
        
        Claim? userIdClaim = httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
        {
            return CommonErrors.Unauthorized;
        }

        // Check if this query is automatically scoped to the organization by global filters
        bool projectExists = await context.Set<Project>()
            .AnyAsync(p => p.Name == request.Name);

        if (projectExists)
        {
            return CreateProjectErrors.ProjectNameAlreadyExists;
        }

        var project = Project.Create(
            request.Name,
            request.Description,
            organizationId,
            userId);

        await context.AddAsync(project);
        await context.SaveChangesAsync();

        return new CreateProjectResponse(
            project.Id,
            project.Name,
            project.Description,
            project.CreatedAt);
    }
}
