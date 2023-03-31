namespace UrlShortener.Domain.Entity;

public class Geolocation : EntityBase
{
    public Guid LinkStatisticId { get; set; }
    public LinkStatistic? LinkStatistic { get; set; }

    public string? Country { get; set; }
    public string? Region { get; set; }
    public string? City { get; set; }
}
