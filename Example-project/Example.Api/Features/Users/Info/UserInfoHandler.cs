using Example.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Example.Api.Features.Users.Info;

internal sealed class UserInfoHandler(UserManager<User> userManager)
{
    public async Task<UserInfoResponse?> HandleAsync(ClaimsPrincipal user)
    {
        string? email = user.FindFirstValue(ClaimTypes.Email) ?? user.FindFirstValue(JwtRegisteredClaimNames.Email);

        if (string.IsNullOrWhiteSpace(email))
        {
            return null;
        }

        User? dbUser = await userManager.FindByEmailAsync(email);

        return dbUser is null ? null : new UserInfoResponse(dbUser.Id, dbUser.Email!, dbUser.UserName!, dbUser.FirstName, dbUser.LastName);
    }
}
