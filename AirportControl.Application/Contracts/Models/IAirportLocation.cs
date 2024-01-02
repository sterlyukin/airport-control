namespace AirportControl.Application.Contracts.Models;

public interface IAirportLocation
{
    double Latitude { get; }
    double Longitude { get; }
}