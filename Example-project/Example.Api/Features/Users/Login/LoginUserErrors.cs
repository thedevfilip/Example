using Example.Domain.Primitives;

namespace Example.Api.Features.Users.Login;

internal static class LoginUserErrors
{
    internal static readonly Error InvalidCredentials = new("5", nameof(InvalidCredentials));
}
