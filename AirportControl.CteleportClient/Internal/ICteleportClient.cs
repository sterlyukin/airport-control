using AirportControl.CteleportClient.Internal.Models;
using Refit;

namespace AirportControl.CteleportClient.Internal;

internal interface ICteleportClient
{
    [Get("/airports/{code}")]
    Task<AirportInformation> GetInformation(string code);
}