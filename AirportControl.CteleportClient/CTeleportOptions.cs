namespace AirportControl.CteleportClient;

public sealed record CTeleportOptions
{
    public required string BaseUrl { get; init; }
    public int RetryCount { get; init; }
    public TimeSpan RetryDuration { get; init; }
}