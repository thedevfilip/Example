namespace Example.Api.Options;

public sealed class RateLimitingOptions
{
    public const string SectionName = "RateLimiting";
    
    public bool Enabled { get; init; }
    public int LoginLimit { get; init; }
    public int RegisterLimit { get; init; }
    public int LoginWindowMinutes { get; init; }
    public int RegisterWindowHours { get; init; }
}
