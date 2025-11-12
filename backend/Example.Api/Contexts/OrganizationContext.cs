using Example.Domain.Contexts;

namespace Example.Api.Contexts;

public sealed class OrganizationContext : IOrganizationContext
{
    public Guid OrganizationId { get; set; }
}
