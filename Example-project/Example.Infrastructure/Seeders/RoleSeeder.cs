using Microsoft.AspNetCore.Identity;

namespace Example.Infrastructure.Seeders;

public sealed class RoleSeeder(RoleManager<IdentityRole<Guid>> roleManager)
{
    private static readonly string[] Roles = ["Admin", "Owner", "Manager", "User", "Guest"];

    public async Task SeedAsync()
    {
        foreach (var role in Roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole<Guid>(role));
        }
    }
}