using UrlShortener.Application.Common.Models;

namespace UrlShortener.Api.Infrastructure;

public interface IGeolocationService
{
    Task<Geolocation> GetData(string ip);
}