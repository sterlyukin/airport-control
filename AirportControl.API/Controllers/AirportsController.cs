using AirportControl.API.Contracts.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace AirportControl.API.Controllers;

[ApiController]
[Route("airports")]
[Produces("application/json")]
[Consumes("application/json")]
public sealed class AirportsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AirportsController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }
    
    [HttpGet("distance")]
    public async Task<IActionResult> GetDistance(GetDistanceQuery query)
    {
        return Ok(await _mediator.Send(query));
    }
}