using Microsoft.AspNetCore.Http;

namespace Example.Domain.Interfaces;

public interface IClientInfoProvider
{
    string GetClientIpAddress(HttpContext context);
    string GetUserAgent(HttpContext context);
}
