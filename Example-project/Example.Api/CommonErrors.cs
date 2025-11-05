using Example.Domain.Primitives;

namespace Example.Api;

internal static class CommonErrors
{
    internal static readonly Error InternalServerError = new("1", nameof(InternalServerError));
    internal static readonly Error Unauthorized = new(nameof(Unauthorized), nameof(Unauthorized));
}
