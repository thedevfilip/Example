using Example.Domain.Entities;
using Example.Domain.Interfaces;
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
    internal async Task<RefreshTokenResponse?> HandleAsync(RefreshTokenRequest request)
    {
        RefreshToken? oldRefreshToken = await context.Set<RefreshToken>()
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.Token == request.RefreshToken);

        if (oldRefreshToken is null || !oldRefreshToken.IsActive)
        {
            // TODO: Respond with valid error code and message
            return null;
        }

        HttpContext? httpContext = httpContextAccessor.HttpContext!;

        string? clientIp = clientInfoProvider.GetClientIpAddress(httpContext);
        string? userAgent = clientInfoProvider.GetUserAgent(httpContext);

        if (!oldRefreshToken.IsValidForClient(clientIp, userAgent))
        {
            oldRefreshToken.Revoke();
            await context.SaveChangesAsync();
            return null;
        }

        var newRefreshToken = RefreshToken.Create(TokenProvider.CreateRefreshToken(), oldRefreshToken.User!, clientIp, userAgent);

        await context.AddAsync(newRefreshToken);
        oldRefreshToken.Revoke();
        await context.SaveChangesAsync();

        IEnumerable<string> roles = await userManager.GetRolesAsync(oldRefreshToken.User!);
        string accessToken = tokenProvider.Create(oldRefreshToken.User!, roles);

        return new RefreshTokenResponse(accessToken, newRefreshToken.Token);
    }
}
