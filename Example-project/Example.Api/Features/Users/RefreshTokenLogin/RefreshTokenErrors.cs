using Example.Domain.Primitives;

namespace Example.Api.Features.Users.RefreshTokenLogin;

internal static class RefreshTokenErrors
{
    internal static readonly Error InvalidRefreshToken = new("3", nameof(InvalidRefreshToken));
    internal static readonly Error RefreshTokenExpired = new("4", nameof(RefreshTokenExpired));
}
