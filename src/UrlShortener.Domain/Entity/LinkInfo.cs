namespace UrlShortener.Domain.Entity;


public class LinkStatistic : EntityBase
{
    public Guid LinkId { get; set; }
    public Link? Link { get; set; }
    public string? DomainName { get; set; }
    public DateTime LastUse { get; set; }
    public int Clicks { get; set; }
    // add some prop
}

