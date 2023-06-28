using System.Text.Json.Serialization;

namespace UrlShortener.Application.Common.Domain;

public class Geolocation
{
    [JsonPropertyName("country")]
    public string? Country { get; set; }
    [JsonPropertyName("regionname")]
    public string? Region { get; set; }
    [JsonPropertyName("city")]
    public string? City { get; set; }
}

