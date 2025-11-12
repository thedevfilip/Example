using Example.Domain.Entities;
using Example.Domain.Interfaces;
using Example.Domain.Primitives;
using Example.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Example.Api.Features.Users.RefreshTokenLogin;

internal sealed class RefreshTokenHandler(
    UserManager<User> userManager,
    TokenProvider tokenProvider,
    AppDbContext context,
    IClientInfoProvider clientInfoProvider,
    IHttpContextAccessor httpContextAccessor)
{
    internal async Task<Result<RefreshTokenResponse>> HandleAsync(RefreshTokenRequest request)
    {
        RefreshToken? oldRefreshToken = await context.Set<RefreshToken>()
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.Token == request.RefreshToken);

        if (oldRefreshToken is null)
        {
            return RefreshTokenErrors.InvalidRefreshToken;
        }

        if (oldRefreshToken.IsRevoked)
        {
            IEnumerable<RefreshToken> tokenFamily = await context.Set<RefreshToken>()
                .Where(p => p.UserId == oldRefreshToken.UserId)
                .ToListAsync();

            foreach (RefreshToken token in tokenFamily)
            {
                token.Revoke();
            }

            await context.SaveChangesAsync();
            return RefreshTokenErrors.InvalidRefreshToken;
        }

        if (oldRefreshToken.Expired)
        {
            return RefreshTokenErrors.RefreshTokenExpired;
        }

        HttpContext? httpContext = httpContextAccessor.HttpContext!;

        string? clientIp = clientInfoProvider.GetClientIpAddress(httpContext);
        string? userAgent = clientInfoProvider.GetUserAgent(httpContext);

        if (!oldRefreshToken.IsValidForClient(clientIp, userAgent))
        {
            oldRefreshToken.Revoke();

            await context.SaveChangesAsync();

            return RefreshTokenErrors.InvalidRefreshToken;
        }

        try
        {
            var newRefreshToken = RefreshToken.Create(TokenProvider.CreateRefreshToken(), oldRefreshToken.User!, clientIp, userAgent);

            await context.AddAsync(newRefreshToken);

            oldRefreshToken.Revoke();

            await context.SaveChangesAsync();

            IEnumerable<string> roles = await userManager.GetRolesAsync(oldRefreshToken.User!);

            string accessToken = tokenProvider.Create(oldRefreshToken.User!, roles);

            return new RefreshTokenResponse(accessToken, newRefreshToken.Token);
        }
        catch (Exception)
        {
            return CommonErrors.InternalServerError;
        }
    }
}
