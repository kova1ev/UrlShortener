namespace UrlShortener.Api.Authentication;

public class JwtOptions
{
    public const string ConfigKey = "JwtOptions";

    public string? Issuer { get; set; }
    public string? Audience { get; set; }
    public string? SecretKey { get; set; }
    public int LifeTime { get; set; }
}