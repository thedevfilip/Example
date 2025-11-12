namespace Example.Api.Features.Projects.CreateProject;

internal sealed record CreateProjectResponse(Guid Id, string Name, string? Description, DateTime CreatedAt);
