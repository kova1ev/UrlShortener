namespace UrlShortener.Domain.Entity;


public class LinkInfo : EntityBase
{
    public Guid LinkId { get; set; }
    public Link Link { get; set; }
    public string DomainName { get; set; }
    public DateTime LastUse { get; set; }

    // add some prop
}

