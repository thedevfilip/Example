namespace Example.Domain.Entities;

public sealed class RefreshToken
{
    public Guid Id { get; init; }
    public string Token { get; private set; } = default!;
    public Guid UserId { get; init; }
    public DateTime ExpiresAt { get; private set; }

    public User User { get; set; } = default!;


    public static RefreshToken Create(string value, User user) =>
        new()
        {
            Id = Guid.NewGuid(),
            Token = value,
            UserId = user.Id,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        };

    public bool Expired => DateTime.UtcNow >= ExpiresAt;

    public void UpdateToken(string value)
    {
        Token = value;
        ExpiresAt = DateTime.UtcNow.AddDays(7);
    }
}
