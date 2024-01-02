namespace AirportControl.CacheClient;

public sealed record CacheOptions
{
    public required string BaseUrl { get; init; }
    public TimeSpan LifeTime { get; init; }
}