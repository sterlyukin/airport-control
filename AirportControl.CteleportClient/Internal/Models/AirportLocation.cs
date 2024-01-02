using System.Text.Json.Serialization;
using AirportControl.Application.Contracts.Models;

namespace AirportControl.CteleportClient.Internal.Models;

internal sealed record AirportLocation : IAirportLocation
{
    [JsonPropertyName("lat")]
    public required double Latitude { get; init; }
    
    [JsonPropertyName("lon")]
    public required double Longitude { get; init; }
}