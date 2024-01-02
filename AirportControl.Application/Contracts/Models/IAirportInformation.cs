namespace AirportControl.Application.Contracts.Models;

public interface IAirportInformation
{
    string Code { get; }
    IAirportLocation Location { get; }
}