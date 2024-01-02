using AirportControl.Application.Queries.GetDistance.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace AirportControl.API.Contracts.Queries;

public sealed record GetDistanceQuery : IGetDistanceQuery
{
    [FromQuery(Name = "source")]
    public required string Source { get; init; }
    
    [FromQuery(Name = "target")]
    public required string Target { get; init; }
}