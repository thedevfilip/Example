using Example.Api.Features.Users.Registration;
using Example.Domain.Entities;
using Example.Infrastructure;
using Microsoft.AspNetCore.Identity;

namespace Example.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddIdentity<User, IdentityRole<Guid>>(options =>
        {
            // Configure password, lockout, and user settings as needed
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequiredLength = 6;
            options.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();

        services.AddAuthentication();
        services.AddAuthorization();

        services.AddScoped<RegisterUserHandler>();

        return services;
    }
}