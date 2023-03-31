using System.Text.Json;
using UrlShortener.Application.Common.Models;

namespace UrlShortener.Api.Infrastructure;

public class GeolocationService : IGeolocationService
{
    private JsonSerializerOptions _options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
    private readonly string apiAddress = "http://ip-api.com/json/";
    private readonly HttpClient _httpClient;

    public GeolocationService(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(_httpClient));
        _httpClient.BaseAddress = new Uri(apiAddress);
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
    }


    public async Task<Geolocation> GetData(string ip)
    {
        if (ip == null)
            throw new ArgumentNullException(nameof(ip));
        string requestQueryParameters = "?fields=status,message,country,regionName,city";
        HttpResponseMessage responseMessage = await _httpClient.GetAsync(string.Concat(ip, requestQueryParameters));
        string jsonString = await responseMessage.Content.ReadAsStringAsync();
        if (!responseMessage.IsSuccessStatusCode)
        {
            throw new BadHttpRequestException($"{apiAddress} return not Success StatusCode", (int)responseMessage.StatusCode);
        }
        Geolocation geolocation = JsonSerializer.Deserialize<Geolocation>(jsonString, _options)!;
        return geolocation;
    }

}


