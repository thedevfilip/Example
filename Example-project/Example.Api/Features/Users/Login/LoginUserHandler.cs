using Example.Api.Options;
using Example.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Example.Api.Features.Users.Login;

internal sealed class LoginUserHandler(
    SignInManager<User> signInManager,
    UserManager<User> userManager,
    IOptionsMonitor<JwtOptions> configuration)
{
    private readonly SignInManager<User> signInManager = signInManager;
    private readonly UserManager<User> userManager = userManager;
    private readonly JwtOptions jwtOptions = configuration.CurrentValue;

    public async Task<LoginUserResponse?> HandleAsync(LoginUserRequest request)
    {
        User? user = await userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            return null;
        }

        SignInResult result = await signInManager.CheckPasswordSignInAsync(user, request.Password, false);
        if (!result.Succeeded)
        {
            return null;
        }

        string token = GenerateJwtToken(user);
        return new LoginUserResponse(user.Email!, token);
    }

    private string GenerateJwtToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        Claim[] claims =
        [
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email!),
            new(ClaimTypes.Name, user.UserName!)
        ];

        var token = new JwtSecurityToken(
            issuer: jwtOptions.Issuer,
            audience: jwtOptions.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
