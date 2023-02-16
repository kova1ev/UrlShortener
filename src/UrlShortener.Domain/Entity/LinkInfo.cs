namespace UrlShortener.Domain.Entity;

public class LinkInfo : EntityBase
{
    public LinkInfo(DateTime lastUse, string domainName, Link link)
    {
        Link = link;
        LinkId = link.Id;
        LastUse = lastUse;
        DomainName = domainName; //GetDomainName(urlAddress) ?? throw new ArgumentNullException(nameof(urlAddress));
    }

    public Guid LinkId { get; set; }
    public Link Link { get; private set; }
    public string DomainName { get; set; }
    public DateTime? LastUse { get; set; } //= DateTime.UtcNow;

    //public LinkInfo(string urlAddress)
    //{
    //    DomainName = GetDomainName(urlAddress);
    //}



    private string GetDomainName(string url)
    {
        var array = url.Split('/');
        if (array.Length < 3)
            throw new Exception($"invalid url : {url}");
        return array[2];
    }
}

