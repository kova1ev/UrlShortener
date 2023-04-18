namespace UrlShortener.Application.Interfaces;

public interface ISystemDateTime
{
    public DateTime UtcNow { get; }
}
