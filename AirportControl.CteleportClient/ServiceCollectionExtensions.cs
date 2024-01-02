using AirportControl.Application.Contracts.Providers;
using AirportControl.CteleportClient.Internal;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Refit;

namespace AirportControl.CteleportClient;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCTeleportClient(
        this IServiceCollection services,
        CTeleportOptions options)
    {
        if (options is null)
            throw new ArgumentNullException(nameof(options));

        services
            .AddScoped<ICteleportProvider, CTeleportProvider>()
            .AddRefitClient<ICteleportClient>()
            .ConfigureHttpClient(c =>
            {
                c.BaseAddress = new Uri(options.BaseUrl);
            })
            .AddTransientHttpErrorPolicy(
                b => b.WaitAndRetryAsync(options.RetryCount, _ => options.RetryDuration)
            );
            
        return services;
    }
}