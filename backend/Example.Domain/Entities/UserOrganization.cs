using Microsoft.AspNetCore.Identity;

namespace Example.Domain.Entities;

public class UserOrganization
{
    public Guid UserId { get; init; }
    public User User { get; init; } = default!;

    public Guid OrganizationId { get; init; }
    public Organization Organization { get; init; } = default!;

    public Guid RoleId { get; init; }
    public IdentityRole<Guid> Role { get; init; } = default!;

    public static UserOrganization Create(Guid userId, Guid organizationId, Guid roleId) =>
        new()
        {
            UserId = userId,
            OrganizationId = organizationId,
            RoleId = roleId
        };
}
