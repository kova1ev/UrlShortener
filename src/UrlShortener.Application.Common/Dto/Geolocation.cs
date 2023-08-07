using System.Text.Json.Serialization;

namespace UrlShortener.Application.Common.Dto;

public class Geolocation
{
    [JsonPropertyName("country_name")] 
    public string? Country { get; set; }
    [JsonPropertyName("state_prov")] 
    public string? Region { get; set; }
    [JsonPropertyName("city")] 
    public string? City { get; set; }
}