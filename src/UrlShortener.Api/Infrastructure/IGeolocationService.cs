using UrlShortener.Application.Common.Domain;

namespace UrlShortener.Api.Infrastructure;

public interface IGeolocationService
{
    Task<Geolocation> GetData(string ip);
}