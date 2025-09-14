using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Example.Infrastructure.Seeders;

namespace Example.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
            x => x.MigrationsAssembly(Constants.MigrationsAssembly)));

        services.AddScoped<RoleSeeder>();
        services.AddScoped<DatabaseSeeder>();

        return services;
    }
}