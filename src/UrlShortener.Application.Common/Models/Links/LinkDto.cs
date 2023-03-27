using UrlShortener.Domain.Entity;

namespace UrlShortener.Application.Common.Models.Links;

public class LinkDto
{
    public Guid Id { get; set; }
    public string? UrlAddress { get; set; }
    public string? UrlShort { get; set; }

    public DateTime DateTimeCreated { get; set; }

    public LinkStatisticDto LinkStatistic { get; set; } = new LinkStatisticDto();
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

                LinkStatistic = new LinkStatisticDto()
                {
                    DomainName = link.LinkStatistic.DomainName,
                    Id = link.LinkStatistic.Id,
                    LastUse = link.LinkStatistic.LastUse,
                    Clicks = link.LinkStatistic.Clicks
                }
            };
        }
        return null;
    }
}

public class LinkStatisticDto
{
    public Guid Id { get; set; }
    public string? DomainName { get; set; }
    public DateTime LastUse { get; set; }
    public int Clicks { get; set; }
}
