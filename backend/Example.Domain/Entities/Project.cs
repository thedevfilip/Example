using Example.Domain.Primitives;

namespace Example.Domain.Entities;

public sealed class Project : IMustHaveTenant
{
    public Guid Id { get; init; }
    public required string Name { get; init; }
    public string? Description { get; private set; }
    public Guid OrganizationId { get; init; }
    public Guid CreatedBy { get; init; }
    public DateTime CreatedAt { get; init; }

    private Project() { }

    public static Project Create(string name, string? description, Guid organizationId, Guid createdBy) =>
        new()
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description,
            OrganizationId = organizationId,
            CreatedBy = createdBy,
            CreatedAt = DateTime.UtcNow
        };
}
