using System.ComponentModel.DataAnnotations.Schema;

namespace UrlShortener.Domain.Entity;

[Table("linkinfos")]
public class LinkInfo : EntityBase
{
    public Guid LinkId { get; set; }
    public Link Link { get; set; }
    public string DomainName { get; set; }
    public DateTime LastUse { get; set; }


    //TODO   CHECK URL? THROW EX?
    public string GetDomainName(string url)
    {
        if (url == null)
            throw new ArgumentNullException(nameof(url));
        var array = url.Split('/');
        if (array.Length < 3)
            throw new Exception($"invalid url : {url}");
        return array[2];
    }
}

