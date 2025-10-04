using System.Text;
using Example.Api.Features.Organizations.Registration;
using Example.Api.Features.Users.Info;
using Example.Api.Features.Users.Login;
using Example.Api.Features.Users.Registration;
using Example.Api.Options;
using Example.Domain.Entities;
using Example.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Example.Api;

internal static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtOptions>(configuration.GetSection(Constants.Jwt));

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

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(Constants.Bearer, options =>
            {
                IConfigurationSection jwtSettings = configuration.GetSection(Constants.Jwt);
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings[Constants.Issuer],
                    ValidAudience = jwtSettings[Constants.Audience],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSettings["Key"]!))
                };
            });

        services.AddAuthorization();

        services.AddScoped<RegisterUserHandler>();
        services.AddScoped<LoginUserHandler>();
        services.AddScoped<UserInfoHandler>();
        services.AddScoped<RegisterOrganizationHandler>();

        return services;
    }
}
