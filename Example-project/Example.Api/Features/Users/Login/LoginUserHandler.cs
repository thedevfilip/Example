using Example.Api.Options;
using Example.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Example.Api.Features.Users.Login;

internal sealed class LoginUserHandler(
    SignInManager<User> signInManager,
    UserManager<User> userManager,
    JwtOptions options)
{
    private readonly SignInManager<User> signInManager = signInManager;
    private readonly UserManager<User> userManager = userManager;
    private readonly JwtOptions options = options;

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

        string token = await GenerateJwtToken(user);
        return new LoginUserResponse(user.Email!, token);
    }

    private async Task<string> GenerateJwtToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Key));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        IList<string> roles = await userManager.GetRolesAsync(user);

        Claim[] claims =
        [
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email!),
            new(ClaimTypes.Name, user.UserName!),
            ..roles.Select(role => new Claim(ClaimTypes.Role, role))
        ];

        var token = new JwtSecurityToken(
            issuer: options.Issuer,
            audience: options.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(options.ExpirationInMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
