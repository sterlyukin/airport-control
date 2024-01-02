using System.Text.Json;
using AirportControl.Application.Contracts.Providers;
using Microsoft.Extensions.Caching.Distributed;

namespace AirportControl.CacheClient.Internal;

internal sealed class CacheProvider : ICacheProvider
{
    private readonly IDistributedCache _cache;
    private readonly CacheOptions _options;

    public CacheProvider(
        IDistributedCache cache,
        CacheOptions options)
    {
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        _options = options ?? throw new ArgumentNullException(nameof(options));
    }

    public async Task Set(object key, object value)
    {
        await _cache.SetStringAsync(JsonSerializer.Serialize(key), JsonSerializer.Serialize(value), new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = _options.LifeTime
        });
    }

    public async Task<TType?> Get<TType>(object key)
    {
        var serializedValue = await _cache.GetStringAsync(JsonSerializer.Serialize(key));
        return string.IsNullOrWhiteSpace(serializedValue) ? default : JsonSerializer.Deserialize<TType>(serializedValue);
    }
}