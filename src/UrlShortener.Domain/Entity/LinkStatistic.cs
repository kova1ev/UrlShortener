namespace UrlShortener.Domain.Entity;


public class LinkStatistic : EntityBase
{
    public string? DomainName { get; set; }
    public DateTime? LastUse { get; set; }
    public int Clicks { get; set; }
    public string? Browser { get; set; }
    public string? Os { get; set; }
    public Geolocation? Geolocation { get; set; }

    public Guid LinkId { get; set; }
    public Link Link { get; set; } = null!;

}

