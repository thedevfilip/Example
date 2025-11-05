using Example.Domain.Primitives;

namespace Example.Api.Features.Users.RefreshTokenLogin;

internal static class RefreshTokenErrors
{
    internal static readonly Error InvalidRefreshToken = new(nameof(InvalidRefreshToken), "Invalid refresh token");
    internal static readonly Error RefreshTokenExpired = new(nameof(RefreshTokenExpired), "Refresh token expired");
}
