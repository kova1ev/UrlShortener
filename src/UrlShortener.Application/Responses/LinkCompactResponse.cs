using UrlShortener.Entity;

namespace UrlShortener.Application.Responses;

public class LinkCompactResponse
{
    public Guid Id { get; set; }
    public string? UrlAddress { get; set; }
    public string? Alias { get; set; }
    public string? UrlShort { get; set; }
    public DateTime DateTimeCreated { get; set; }

    public static LinkCompactResponse MapFromLink(Link link)
    {
        return new LinkCompactResponse()
        {
            Id = link.Id,
            UrlAddress = link.UrlAddress,
            Alias = link.Alias,
            UrlShort = link.UrlShort,
            DateTimeCreated = link.DateTimeCreated
        };
    }
}
