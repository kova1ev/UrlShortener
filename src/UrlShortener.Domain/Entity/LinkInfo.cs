using System.ComponentModel.DataAnnotations.Schema;

namespace UrlShortener.Domain.Entity;

[Table("linkinfos")]
public class LinkInfo : EntityBase
{
    public Guid LinkId { get; set; }
    public Link Link { get; set; }
    public string DomainName { get; set; }
    public DateTime LastUse { get; set; }

    // add some prop
}

