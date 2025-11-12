using Example.Domain.Entities;
using Example.Domain.Primitives;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Example.Api.Features.Users.Info;

internal sealed class UserInfoHandler(UserManager<User> userManager)
{
    public async Task<Result<UserInfoResponse>> HandleAsync(ClaimsPrincipal user)
    {
        string? email = user.FindFirstValue(ClaimTypes.Email) ?? user.FindFirstValue(JwtRegisteredClaimNames.Email);

        if (string.IsNullOrWhiteSpace(email))
        {
            return CommonErrors.InternalServerError;
        }

        User? dbUser = await userManager.FindByEmailAsync(email);

        if (dbUser is null)
        {
            return UserInfoErrors.NonExistingUser;
        }

        return new UserInfoResponse(dbUser.Id, dbUser.Email!, dbUser.UserName!, dbUser.FirstName, dbUser.LastName);
    }
}
