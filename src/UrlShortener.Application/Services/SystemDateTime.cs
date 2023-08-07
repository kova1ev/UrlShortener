using UrlShortener.Application.Interfaces;

namespace UrlShortener.Application.Services;

public class SystemDateTime : ISystemDateTime
{
    public DateTime UtcNow => DateTime.UtcNow;
    public DateTime Now => DateTime.Now;
}