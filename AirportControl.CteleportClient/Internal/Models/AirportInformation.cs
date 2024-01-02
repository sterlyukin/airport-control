using System.Text.Json.Serialization;
using AirportControl.Application.Contracts.Models;

namespace AirportControl.CteleportClient.Internal.Models;

internal sealed record AirportInformation : IAirportInformation
{
    [JsonPropertyName("iata")]
    public required string Code { get; init; }
    
    [JsonPropertyName("location")]
    public required AirportLocation Location { get; init; }
    
    IAirportLocation IAirportInformation.Location => Location;
}