namespace AirportControl.Application.Contracts.Providers;

public interface ICacheProvider
{
    Task Set(object key, object value);
    Task<TType?> Get<TType>(object key);
}