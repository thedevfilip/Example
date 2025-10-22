using Example.Domain.Primitives;

namespace Example.Api.Features.Users.Registration;

internal static class RegistrationErrors
{
    internal static readonly Error InternalServerError = new("1", nameof(InternalServerError));
    internal static readonly Error EmailTaken = new("2", nameof(EmailTaken));
}
