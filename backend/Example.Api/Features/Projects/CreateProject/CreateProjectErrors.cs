using Example.Domain.Primitives;

namespace Example.Api.Features.Projects.CreateProject;

internal static class CreateProjectErrors
{
    internal static readonly Error ProjectNameAlreadyExists = new(nameof(ProjectNameAlreadyExists), "A project with this name already exists in your organization");
    internal static readonly Error InvalidProjectName = new(nameof(InvalidProjectName), "Project name cannot be empty and must be less than 200 characters");
}
