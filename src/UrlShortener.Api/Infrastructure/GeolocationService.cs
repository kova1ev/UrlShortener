using System.Text.Json;
using UrlShortener.Application.Common.Domain;
using UrlShortener.Application.Interfaces;

namespace UrlShortener.Api.Infrastructure;

/// <inheritdoc />
public class GeolocationService : IGeolocationService
{
    private const string ApiAddress = "http://ip-api.com/json/";
    private readonly TimeSpan _timeoutRequest = TimeSpan.FromSeconds(30);
    private readonly HttpClient _httpClient;
    private readonly ILogger<GeolocationService> _logger;
    private readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };
    private readonly CancellationTokenSource _cancellationTokenSource = new();

    public GeolocationService(HttpClient httpClient, ILogger<GeolocationService> logger)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _httpClient.BaseAddress = new Uri(ApiAddress);
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
            var requestQueryParameters = "?fields=status,message,country,regionName,city";
            var responseMessage =
                await _httpClient.GetAsync(string.Concat(ip, requestQueryParameters), cancellationToken);
            var jsonString = await responseMessage.Content.ReadAsStringAsync(cancellationToken);
            if (!responseMessage.IsSuccessStatusCode)
                throw new BadHttpRequestException($"{ApiAddress} with ip {ip} return not Success StatusCode",
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