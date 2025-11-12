namespace Example.Api.Features.Projects.GetProject;

internal sealed record GetProjectResponse(Guid Id, string Name, string? Description, DateTime CreatedAt);
