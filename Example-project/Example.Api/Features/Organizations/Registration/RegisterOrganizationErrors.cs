using Example.Domain.Primitives;

namespace Example.Api.Features.Organizations.Registration;

internal static class RegisterOrganizationErrors
{
    internal static readonly Error InvalidOrganizationName = new(nameof(InvalidOrganizationName), "Invalid organization name");
    internal static readonly Error OrganizationExists = new(nameof(OrganizationExists), "Organization exists");
}
