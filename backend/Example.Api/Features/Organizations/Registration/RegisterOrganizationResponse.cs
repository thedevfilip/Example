namespace Example.Api.Features.Organizations.Registration;

internal sealed record RegisterOrganizationResponse(
    Guid OrganizationId,
    string Name
);
