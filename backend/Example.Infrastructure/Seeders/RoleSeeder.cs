using Microsoft.AspNetCore.Identity;

namespace Example.Infrastructure.Seeders;

public sealed class RoleSeeder(RoleManager<IdentityRole<Guid>> roleManager)
{
    private static readonly string[] _roles = ["Admin", "Owner", "Manager", "User", "Guest"];

    public async Task SeedAsync()
    {
        foreach (string role in _roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>(role));
            }
        }
    }
}
