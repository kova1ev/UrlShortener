using UrlShortener.Domain.Entity;

namespace UrlShortener.Application.Common.Models.Links;

public class LinkDto
{
    public Guid Id { get; set; }
    public string? UrlAddress { get; set; }
    public string? UrlShort { get; set; }

    public DateTime DateTimeCreated { get; set; }

    public LinkInfoDto LinkInfo { get; set; } = new LinkInfoDto();
    public LinkDto() { }

    public static LinkDto? MapToLInkDto(Link? link)
    {
        if (link != null)
        {
            return new LinkDto()
            {
                Id = link.Id,
                UrlAddress = link.UrlAddress,
                DateTimeCreated = link.DateTimeCreated,
                UrlShort = link.UrlShort,

                LinkInfo = new LinkInfoDto()
                {
                    DomainName = link.LinkInfo.DomainName,
                    Id = link.LinkInfo.Id,
                    LastUse = link.LinkInfo.LastUse
                }
            };
        }
        return null;
    }
}

public class LinkInfoDto
{
    public Guid Id { get; set; }
    public string? DomainName { get; set; }
    public DateTime LastUse { get; set; }

}
