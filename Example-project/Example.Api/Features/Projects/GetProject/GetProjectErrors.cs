using Example.Domain.Primitives;

namespace Example.Api.Features.Projects.GetProject;

public static class GetProjectErrors
{
    public static readonly Error ProjectNotFound = new(
        nameof(ProjectNotFound), 
        "Project not found");
}
