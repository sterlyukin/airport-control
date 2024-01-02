using AirportControl.Application.Contracts.Providers;
using AirportControl.CacheClient.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace AirportControl.CacheClient;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCache(
        this IServiceCollection services,
        CacheOptions cacheOptions)
    {
        if (cacheOptions is null)
            throw new ArgumentNullException(nameof(cacheOptions));

        return
            services
                .AddSingleton(cacheOptions)
                .AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = cacheOptions.BaseUrl;
                })
                .AddScoped<ICacheProvider, CacheProvider>();
    }
}