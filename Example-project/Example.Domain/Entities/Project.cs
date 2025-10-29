using Example.Domain.Primitives;

namespace Example.Domain.Entities;

public sealed class Project : IMustHaveTenant
{
    public Guid Id { get; init; }
    public string Name { get; private set; } = default!;
    public Guid OrganizationId { get; init; }
}
