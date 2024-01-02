using AirportControl.Application.Contracts.Models;

namespace AirportControl.Application.Contracts.Providers;

public interface ICteleportProvider
{
    Task<IAirportInformation> GetInformation(string code);
}