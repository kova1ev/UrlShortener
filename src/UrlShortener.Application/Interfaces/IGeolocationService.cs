using UrlShortener.Application.Common.Dto;

namespace UrlShortener.Application.Interfaces;

/// <summary>
/// Represents an HTTP service for getting geolocation by id.
/// </summary>
public interface IGeolocationService
{
    /// <summary>
    /// Return geolocation information.
    /// </summary>
    /// <param name="ip">Client Ip.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns> Return geolocation information from remote Api.</returns>
    /// <exception cref="ArgumentNullException">Throw <paramref name="ip"/> is null.</exception>
    Task<Geolocation> GetGeolocationDataAsync(string ip, CancellationToken cancellationToken = default);
}