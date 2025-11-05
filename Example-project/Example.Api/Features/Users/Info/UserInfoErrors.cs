using Example.Domain.Primitives;

namespace Example.Api.Features.Users.Info;

internal static class UserInfoErrors
{
    internal static readonly Error NonExistingUser = new(nameof(NonExistingUser), "User does not exist");
}
