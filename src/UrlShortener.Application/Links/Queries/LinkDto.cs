using UrlShortener.Domain.Entity;

namespace UrlShortener.Application.Links.Queries;


public class LinkDto
{
    public Guid Id { get; set; }
    public string UrlAddress { get; set; }
    public string UrlShort { get; set; }

    public DateTime DateTimeCreated { get; set; }

    public LinkInfoDto LinkInfo { get; set; } = new LinkInfoDto();

    public LinkDto(Link? link)
    {
        if (link != null)
        {
            UrlAddress = link.UrlAddress;
            DateTimeCreated = link.DateTimeCreated;
            UrlShort = link.UrlShort;

            LinkInfo = new LinkInfoDto()
            {
                DomainName = link.LinkInfo.DomainName,
                Id = link.LinkInfo.Id,
                LastUse = link.LinkInfo.LastUse
            };

        }

    }
}

public class LinkInfoDto
{
    public Guid Id { get; set; }
    public string DomainName { get; set; }
    public DateTime LastUse { get; set; }

}
