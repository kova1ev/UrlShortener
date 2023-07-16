namespace UrlShortener.Application.Common;

public class AppOptions
{
    public const string ConfigKey = "AppOptions";
    public string? AppUrl { get; set; }
    public string? GeoApiKey { get; set; }
}