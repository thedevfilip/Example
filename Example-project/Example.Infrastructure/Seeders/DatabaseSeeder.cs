namespace Example.Infrastructure.Seeders;

public sealed class DatabaseSeeder(RoleSeeder roleSeeder)
{
    public async Task SeedAsync() =>
        await roleSeeder.SeedAsync();
}
