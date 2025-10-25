using System.Security.Claims;
using Example.Domain.Entities;
using Example.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Example.Api.Features.Organizations.Registration;

internal sealed class RegisterOrganizationHandler(
    AppDbContext context,
    IHttpContextAccessor httpContextAccessor,
    RoleManager<IdentityRole<Guid>> roleManager)
{
    public async Task<RegisterOrganizationResponse> HandleAsync(RegisterOrganizationRequest request)
    {
        var userId = Guid.Parse(httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        IdentityRole<Guid> ownerRole = await roleManager.Roles.SingleAsync(r => r.Name == "Owner");

        var organization = Organization.Create(request.Name);

        var userOrganization = UserOrganization.Create(userId, organization.Id, ownerRole.Id);

        context.Add(organization);
        context.Add(userOrganization);

        await context.SaveChangesAsync();

        return new RegisterOrganizationResponse(organization.Id, organization.Name);
    }
}
