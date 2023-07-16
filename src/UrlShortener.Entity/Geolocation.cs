namespace UrlShortener.Entity;

public class Geolocation : EntityBase
{
    public string? Country { get; set; }
    public string? Region { get; set; }
    public string? City { get; set; }

    public Guid LinkStatisticId { get; set; }
    public virtual LinkStatistic LinkStatistic { get; set; } = null!;
}
