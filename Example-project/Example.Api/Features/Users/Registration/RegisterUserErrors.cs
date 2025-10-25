using Example.Domain.Primitives;

namespace Example.Api.Features.Users.Registration;

internal static class RegisterUserErrors
{
    internal static readonly Error EmailTaken = new("2", nameof(EmailTaken));
}
