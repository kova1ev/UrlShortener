using System.Text.Json;
using UrlShortener.Application.Common.Models;

namespace UrlShortener.Api.Infrastructure;

public class GeolocationService : IGeolocationService
{
    private readonly JsonSerializerOptions _options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
    private readonly string apiAddress = "http://ip-api.com/json/";
    private readonly HttpClient _httpClient;
    private readonly ILogger<GeolocationService> _logger;
    public GeolocationService(HttpClient httpClient, ILogger<GeolocationService> logger)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(_httpClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _httpClient.BaseAddress = new Uri(apiAddress);
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
    }


    public async Task<Geolocation> GetData(string ip)
    {
        Geolocation geolocation = new();
        try
        {
            if (ip == null)
                throw new ArgumentNullException(nameof(ip));
            string requestQueryParameters = "?fields=status,message,country,regionName,city";
            HttpResponseMessage responseMessage = await _httpClient.GetAsync(string.Concat(ip, requestQueryParameters));
            string jsonString = await responseMessage.Content.ReadAsStringAsync();
            if (!responseMessage.IsSuccessStatusCode)
            {
                throw new BadHttpRequestException($"{apiAddress} with ip {ip} return not Success StatusCode", (int)responseMessage.StatusCode);
            }
            geolocation = JsonSerializer.Deserialize<Geolocation>(jsonString, _options)!;
        }
        catch (BadHttpRequestException exception)
        {
            _logger.LogError(exception, "Exception occurred: {Message}", exception.Message);
        }
        catch (Exception)
        {
            throw;
        }
        return geolocation;
    }

}


