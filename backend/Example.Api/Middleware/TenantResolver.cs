using System.Security.Claims;
using Example.Api.Contexts;
using Example.Domain.Contexts;

namespace Example.Api.Middleware;

public class TenantResolver(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, IOrganizationContext organizationContext)
    {
        if (context.User.Identity is { IsAuthenticated: true })
        {
            Claim? organizationIdClaim = context.User.FindFirst("organizationId");

            if (organizationIdClaim is not null && Guid.TryParse(organizationIdClaim.Value, out Guid organizationId))
            {
                ((OrganizationContext)organizationContext).OrganizationId = organizationId;
            }
        }

        await next(context);
    }
}
