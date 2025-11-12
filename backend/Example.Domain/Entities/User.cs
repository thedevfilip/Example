using Microsoft.AspNetCore.Identity;

namespace Example.Domain.Entities;

public class User : IdentityUser<Guid>
{
    public string? FirstName { get; private set; }
    public string? LastName { get; private set; }
    public DateTime CreatedAt { get; init; }

    // TODO: Use this property to track user login activity
    public DateTime? LastLoginAt { get; set; }

    public ICollection<UserOrganization> UserOrganizations { get; } = [];

    public static User Create(string email, string firstName, string lastName) =>
        new()
        {
            Id = Guid.NewGuid(),
            UserName = email,
            Email = email,
            FirstName = firstName,
            LastName = lastName,
            CreatedAt = DateTime.UtcNow
        };
}
