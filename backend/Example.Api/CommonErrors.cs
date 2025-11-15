using Example.Domain.Primitives;

namespace Example.Api;

internal static class CommonErrors
{
    internal static readonly Error InternalServerError = new(nameof(InternalServerError), "Internal server error occurred.");
    internal static readonly Error Unauthorized = new(nameof(Unauthorized), nameof(Unauthorized));
}
