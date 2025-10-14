namespace Example.Domain.Entities;

public sealed class RefreshToken
{
    public Guid Id { get; init; }
    public string Token { get; private set; } = default!;
    public Guid UserId { get; init; }
    public DateTime ExpiresAt { get; private set; }
    public string IpAddress { get; init; } = default!;
    public string UserAgent { get; init; } = default!;
    public DateTime CreatedAt { get; init; }
    public bool IsRevoked { get; private set; }

    public User User { get; set; } = default!;

    public static RefreshToken Create(string value, User user, string ipAddress, string userAgent) =>
        new()
        {
            Id = Guid.NewGuid(),
            Token = value,
            UserId = user.Id,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            IpAddress = ipAddress,
            UserAgent = userAgent,
            CreatedAt = DateTime.UtcNow
        };

    public bool Expired => DateTime.UtcNow >= ExpiresAt;
    public bool IsActive => !Expired && !IsRevoked;

    public void Revoke() => IsRevoked = true;

    public bool IsValidForClient(string clientIp, string clientUserAgent)
    {
        bool ipMatches = string.Equals(IpAddress, clientIp, StringComparison.OrdinalIgnoreCase);
        bool userAgentMatches = string.Equals(UserAgent, clientUserAgent, StringComparison.OrdinalIgnoreCase);

        return ipMatches && userAgentMatches;
    }
}
