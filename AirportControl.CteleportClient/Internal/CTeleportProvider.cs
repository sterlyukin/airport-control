using AirportControl.Application.Contracts.Models;
using AirportControl.Application.Contracts.Providers;
using AirportControl.CteleportClient.Internal.Models;

namespace AirportControl.CteleportClient.Internal;

internal sealed class CTeleportProvider : ICteleportProvider
{
    private readonly ICacheProvider _cacheProvider; 
    private readonly ICteleportClient _cteleportClient;

    public CTeleportProvider(
        ICacheProvider cacheProvider,
        ICteleportClient cteleportClient)
    {
        _cacheProvider = cacheProvider ?? throw new ArgumentNullException(nameof(cacheProvider));
        _cteleportClient = cteleportClient ?? throw new ArgumentNullException(nameof(cteleportClient));
    }
    
    public async Task<IAirportInformation> GetInformation(string code)
    {
        var cachedAirportInformation = await _cacheProvider.Get<AirportInformation>(code);
        if (cachedAirportInformation is not null)
            return cachedAirportInformation;
        
        var airportInformation = await _cteleportClient.GetInformation(code);
        await _cacheProvider.Set(code, airportInformation);
        return airportInformation;
    }
}