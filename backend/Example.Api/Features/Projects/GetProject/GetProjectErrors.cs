using Example.Domain.Primitives;

namespace Example.Api.Features.Projects.GetProject;

internal static class GetProjectErrors
{
    internal static readonly Error ProjectNotFound = new(nameof(ProjectNotFound), "Project not found");
}
