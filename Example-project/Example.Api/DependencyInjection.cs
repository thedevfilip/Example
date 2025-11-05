using System.Text;
using System.Threading.RateLimiting;
using Example.Api.Features.Organizations.Registration;
using Example.Api.Features.Projects.CreateProject;
using Example.Api.Features.Projects.GetProject;
using Example.Api.Features.Users.Info;
using Example.Api.Features.Users.Login;
using Example.Api.Features.Users.RefreshTokenLogin;
using Example.Api.Features.Users.Registration;
using Example.Domain.Entities;
using Example.Domain.Options;
using Example.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Example.Api;

internal static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services, IConfiguration configuration)
    {
        IConfigurationSection jwtSection = configuration.GetSection(Constants.Jwt);

        services.Configure<JwtOptions>(jwtSection);

        services.AddIdentity<User, IdentityRole<Guid>>(options =>
        {
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
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSection[Constants.Issuer],
                ValidAudience = jwtSection[Constants.Audience],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["Key"]!))
            });

        services.AddAuthorization(options =>
            options.FallbackPolicy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .Build());

        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            options.AddPolicy("login-refresh", httpContext => RateLimitPartition.GetFixedWindowLimiter(
                partitionKey: httpContext.Connection.RemoteIpAddress?.ToString(),
                factory: _ => new FixedWindowRateLimiterOptions
                {
                    PermitLimit = 5,
                    Window = TimeSpan.FromMinutes(1)
                }));

            options.AddPolicy("register", httpContext => RateLimitPartition.GetFixedWindowLimiter(
                partitionKey: httpContext.Connection.RemoteIpAddress?.ToString(),
                factory: _ => new FixedWindowRateLimiterOptions
                {
                    PermitLimit = 3,
                    Window = TimeSpan.FromHours(1)
                }));
        });


        services.AddScoped<RegisterUserHandler>();
        services.AddScoped<LoginUserHandler>();
        services.AddScoped<RefreshTokenHandler>();
        services.AddScoped<UserInfoHandler>();
        services.AddScoped<RegisterOrganizationHandler>();

        services.AddScoped<CreateProjectHandler>();
        services.AddScoped<GetProjectHandler>();

        services.AddHttpContextAccessor();

        services.AddSingleton(p => p.GetRequiredService<IOptions<JwtOptions>>().Value);
        services.AddSingleton<TokenProvider>();

        return services;
    }
}
