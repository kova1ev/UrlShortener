using System.ComponentModel.DataAnnotations.Schema;

namespace UrlShortener.Domain.Entity;

[Table("linkinfos")]
public class LinkInfo : EntityBase
{
    //public LinkInfo(DateTime lastUse, Link Link)
    //{
    //    LastUse = lastUse;
    //    this.Link = Link;
    //    LinkId = Link.Id;
    //    LastUse = lastUse;
    //    DomainName = GetDomainName(Link.UrlAddress); //?? throw new ArgumentNullException(nameof(urlAddress));
    //}

    public Guid LinkId { get; set; }
    public Link Link { get; set; }
    public string DomainName { get; set; }
    public DateTime LastUse { get; set; }



    public string GetDomainName(string url)
    {
        var array = url.Split('/');
        if (array.Length < 3)
            throw new Exception($"invalid url : {url}");
        return array[2];
    }
}

