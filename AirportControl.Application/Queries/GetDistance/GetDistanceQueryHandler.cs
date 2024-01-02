using AirportControl.Application.Contracts.Providers;
using AirportControl.Application.Queries.GetDistance.Contracts;
using MediatR;

namespace AirportControl.Application.Queries.GetDistance;

public sealed class GetDistanceQueryHandler : IRequestHandler<IGetDistanceQuery, Distance>
{
    private readonly ICteleportProvider _cteleportProvider;

    public GetDistanceQueryHandler(ICteleportProvider cteleportProvider)
    {
        _cteleportProvider = cteleportProvider ?? throw new ArgumentNullException(nameof(cteleportProvider));
    }
    
    public async Task<Distance> Handle(IGetDistanceQuery request, CancellationToken cancellationToken)
    {
        var airportsInformation = await Task.WhenAll(
            _cteleportProvider.GetInformation(request.Source),
            _cteleportProvider.GetInformation(request.Target));

        var sourceAirportInformation = airportsInformation[Constants.Airports.SourceAirportIndex];
        var targetAirportInformation = airportsInformation[Constants.Airports.TargetAirportIndex];

        var distanceValue = CalculateDistance(
            sourceAirportInformation.Location.Latitude,
            sourceAirportInformation.Location.Longitude,
            targetAirportInformation.Location.Latitude,
            targetAirportInformation.Location.Longitude);

        return new Distance(distanceValue);
    }
    
    private static double CalculateDistance(
        double sourceLatitude,
        double sourceLongitude,
        double targetLatitude,
        double targetLongitude)
    {
        var latitudeDiff = ToRadians(targetLatitude - sourceLatitude);
        var longitudeDiff = ToRadians(targetLongitude - sourceLongitude);
        sourceLatitude = ToRadians(sourceLatitude);
        targetLatitude = ToRadians(targetLatitude);

        var halfChordLengthSquared = Math.Sin(latitudeDiff / 2) * Math.Sin(latitudeDiff / 2) +
                   Math.Sin(longitudeDiff / 2) * Math.Sin(longitudeDiff / 2) * Math.Cos(sourceLatitude) * Math.Cos(targetLatitude);
        var angularDistanceRadians  = 2 * Math.Atan2(Math.Sqrt(halfChordLengthSquared), Math.Sqrt(1 - halfChordLengthSquared));

        return Math.Round(Constants.Calculations.EarthRadiusMiles * angularDistanceRadians , 2);
    }

    private static double ToRadians(double angleInDecimalDegrees)
    {
        return angleInDecimalDegrees * (Math.PI / 180.0);
    }
}