using Example.Domain.Interfaces;
using Example.Infrastructure.Seeders;
using Example.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
        services.AddScoped<IClientInfoProvider, ClientInfoProvider>();

        return services;
    }
}
