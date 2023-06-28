using System.Text.Json;
using UrlShortener.Application.Common.Domain;

namespace UrlShortener.Api.Infrastructure;

public class GeolocationService : IGeolocationService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<GeolocationService> _logger;
    private readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };
    private const string ApiAddress = "http://ip-api.com/json/";

    public GeolocationService(HttpClient httpClient, ILogger<GeolocationService> logger)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _httpClient.BaseAddress = new Uri(ApiAddress);
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
    }


    public async Task<Geolocation> GetData(string ip)
    {
        Geolocation geolocation = new();
        try
        {
            if (ip == null)
                throw new ArgumentNullException(nameof(ip));
            var requestQueryParameters = "?fields=status,message,country,regionName,city";
            var responseMessage = await _httpClient.GetAsync(string.Concat(ip, requestQueryParameters));
            var jsonString = await responseMessage.Content.ReadAsStringAsync();
            if (!responseMessage.IsSuccessStatusCode)
                throw new BadHttpRequestException($"{ApiAddress} with ip {ip} return not Success StatusCode",
                    (int)responseMessage.StatusCode);
            geolocation = JsonSerializer.Deserialize<Geolocation>(jsonString, _options)!;
        }
        catch (BadHttpRequestException exception)
        {
            _logger.LogError(exception, "Exception occurred: {Message}", exception.Message);
        }

        return geolocation;
    }
}