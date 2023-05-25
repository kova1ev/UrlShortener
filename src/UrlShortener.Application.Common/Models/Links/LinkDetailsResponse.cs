using UrlShortener.Domain.Entity;

namespace UrlShortener.Application.Common.Models.Links;

public class LinkDetailsResponse
{
    public Guid Id { get; set; }
    public string? UrlAddress { get; set; }
    public string? UrlShort { get; set; }
    public DateTime DateTimeCreated { get; set; }
    public LinkStatisticResponse LinkStatistic { get; set; } = new LinkStatisticResponse();

    public static LinkDetailsResponse? MapToLInkDto(Link? link)
    {
        if (link != null)
        {
            return new LinkDetailsResponse()
            {
                Id = link.Id,
                UrlAddress = link.UrlAddress,
                DateTimeCreated = link.DateTimeCreated,
                UrlShort = link.UrlShort,

                LinkStatistic = new LinkStatisticResponse()
                {
                    DomainName = link.LinkStatistic?.DomainName,
                    Id = link.LinkStatistic!.Id,
                    LastUse = link.LinkStatistic?.LastUse,
                    Clicks = link.LinkStatistic!.Clicks,
                    Browser = link.LinkStatistic?.Browser,
                    Os = link.LinkStatistic?.Os,

                    City = link.LinkStatistic?.Geolocation?.City,
                    Country = link.LinkStatistic?.Geolocation?.Country,
                    Region = link.LinkStatistic?.Geolocation?.Region
                }
            };
        }
        return null;
    }
}

public class LinkStatisticResponse
{
    public Guid Id { get; set; }
    public string? DomainName { get; set; }
    public DateTime? LastUse { get; set; }
    public int Clicks { get; set; }
    public string? Browser { get; set; }
    public string? Os { get; set; }
    public string? Country { get; set; }
    public string? Region { get; set; }
    public string? City { get; set; }
}
