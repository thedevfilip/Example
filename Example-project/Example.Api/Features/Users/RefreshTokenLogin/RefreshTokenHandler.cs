using Example.Domain.Entities;
using Example.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Example.Api.Features.Users.RefreshTokenLogin;

internal sealed class RefreshTokenHandler(
    UserManager<User> userManager,
    AppDbContext context,
    TokenProvider tokenProvider)
{
    internal async Task<RefreshTokenResponse?> HandleAsync(RefreshTokenRequest request)
    {
        RefreshToken? refreshToken = await context.Set<RefreshToken>()
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.Token == request.RefreshToken);

        if (refreshToken is null || refreshToken.Expired)
        {
            // TODO: Respond with valid error code and message
            return null;
        }

        IEnumerable<string> roles = await userManager.GetRolesAsync(refreshToken.User);

        string accessToken = tokenProvider.Create(refreshToken.User, roles);

        refreshToken.UpdateToken(TokenProvider.CreateRefreshToken());

        await context.SaveChangesAsync();

        return new RefreshTokenResponse(accessToken, refreshToken.Token);
    }
}
