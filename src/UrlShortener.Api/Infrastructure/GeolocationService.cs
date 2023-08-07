using System.Text.Json;
using Microsoft.Extensions.Options;
using UrlShortener.Application.Common;
using UrlShortener.Application.Common.Dto;
using UrlShortener.Application.Interfaces;

namespace UrlShortener.Api.Infrastructure;

/// <inheritdoc />
public class GeolocationService : IGeolocationService
{
    private const string BaseAddress = "https://api.ipgeolocation.io/ipgeo";
    private readonly TimeSpan _timeoutRequest = TimeSpan.FromSeconds(30);
    private readonly HttpClient _httpClient;
    private readonly ILogger<GeolocationService> _logger;
    private readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };
    private readonly CancellationTokenSource _cancellationTokenSource = new();
    private readonly AppOptions _appOptions;

    public GeolocationService(HttpClient httpClient, ILogger<GeolocationService> logger, IOptions<AppOptions> options)
    {
        _appOptions = options.Value ?? throw new ArgumentNullException(nameof(options));
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _httpClient.BaseAddress = new Uri(BaseAddress);
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        _cancellationTokenSource.CancelAfter(_timeoutRequest);
    }
    
    public async Task<Geolocation> GetGeolocationDataAsync(string ip, CancellationToken cancellationToken)
    {
        Geolocation geolocation = new();
        if (ip == null)
            throw new ArgumentNullException(nameof(ip));

        if (cancellationToken == default)
        {
            _cancellationTokenSource.CancelAfter(_timeoutRequest);
            cancellationToken = _cancellationTokenSource.Token;
        }

        try
        {
            var queryParams = $"?apiKey={_appOptions.GeoApiKey}&ip={ip}&fields=geo";
            var responseMessage = await _httpClient.GetAsync(queryParams, cancellationToken);
            var jsonString = await responseMessage.Content.ReadAsStringAsync(cancellationToken);
            if (!responseMessage.IsSuccessStatusCode)
                throw new BadHttpRequestException($"{BaseAddress} with ip {ip} return not Success StatusCode",
                    (int)responseMessage.StatusCode);
            geolocation = JsonSerializer.Deserialize<Geolocation>(jsonString, _options)!;
        }
        catch (BadHttpRequestException exception)
        {
            _logger.LogError(exception, "Exception occurred: {@Message}", exception.Message);
        }
        catch (OperationCanceledException exception)
        {
            _logger.LogError(exception, "Request aborted after {@Timeout} sec : {@Message}", _timeoutRequest,
                exception.Message);
        }

        return geolocation;
    }
}