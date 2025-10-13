using Example.Domain.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Example.Infrastructure.Services;

public class ClientInfoProvider : IClientInfoProvider
{
    public string GetClientIpAddress(HttpContext context) => context.Connection.RemoteIpAddress?.ToString() ?? "Unknown";

    public string GetUserAgent(HttpContext context) => context.Request.Headers["User-Agent"].FirstOrDefault() ?? "Unknown";
}
