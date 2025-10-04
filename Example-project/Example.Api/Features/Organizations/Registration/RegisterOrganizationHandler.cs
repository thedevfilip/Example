using Example.Domain.Entities;
using Example.Infrastructure;

namespace Example.Api.Features.Organizations.Registration;

internal sealed class RegisterOrganizationHandler(AppDbContext context)
{
    public async Task<RegisterOrganizationResponse> HandleAsync(RegisterOrganizationRequest request)
    {
        Organization organization = new(Guid.NewGuid(), request.Name);
        context.Set<Organization>().Add(organization);
        await context.SaveChangesAsync();
        return new RegisterOrganizationResponse(organization.Id, organization.Name);
    }
}
