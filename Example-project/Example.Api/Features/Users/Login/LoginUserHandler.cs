using Example.Api.Options;
using Example.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Example.Api.Features.Users.Login;

public sealed class LoginUserHandler(
    SignInManager<User> signInManager,
    UserManager<User> userManager,
    IOptionsMonitor<JwtOptions> configuration)
{
    private readonly SignInManager<User> signInManager = signInManager;
    private readonly UserManager<User> userManager = userManager;
    private readonly JwtOptions jwtOptions = configuration.CurrentValue;

    public async Task<LoginUserResponse?> HandleAsync(LoginUserRequest request)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null)
            return null;

        var result = await signInManager.CheckPasswordSignInAsync(user, request.Password, false);
        if (!result.Succeeded)
            return null;

        var token = GenerateJwtToken(user);
        return new LoginUserResponse(user.Email!, token);
    }

    private string GenerateJwtToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim(ClaimTypes.Name, user.UserName!)
        };

        var token = new JwtSecurityToken(
            issuer: jwtOptions.Issuer,
            audience: jwtOptions.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}