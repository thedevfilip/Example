using System.Security.Claims;
using Example.Domain.Entities;
using Example.Domain.Primitives;
using Example.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Example.Api.Features.Organizations.Registration;

internal sealed class RegisterOrganizationHandler(
    AppDbContext context,
    IHttpContextAccessor httpContextAccessor,
    RoleManager<IdentityRole<Guid>> roleManager)
{
    public async Task<Result<RegisterOrganizationResponse>> HandleAsync(RegisterOrganizationRequest request)
    {
        if(string.IsNullOrEmpty(request.Name) || request.Name.Length > 200)
        {
            return RegisterOrganizationErrors.InvalidOrganizationName;
        }

        var userId = Guid.Parse(httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        IdentityRole<Guid> ownerRole = await roleManager.Roles.SingleAsync(r => r.Name == "Owner");

        bool organizationExists = await context.Set<Organization>()
            .AnyAsync(p => p.Name == request.Name);

        if(organizationExists)
        {
            return RegisterOrganizationErrors.OrganizationExists;
        }

        var organization = Organization.Create(request.Name);

        var userOrganization = UserOrganization.Create(userId, organization.Id, ownerRole.Id);

        context.Add(organization);
        context.Add(userOrganization);

        await context.SaveChangesAsync();

        return new RegisterOrganizationResponse(organization.Id, organization.Name);
    }
}
