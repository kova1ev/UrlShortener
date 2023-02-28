namespace UrlShortener.Application.Links.Commands.CreateLink;

public sealed class LinkResponse
{
    public string Link { get; set; }
    public Guid Id { get; set; }
    public LinkResponse(Guid id, string shortUrl)
    {
        Id = id;
        Link = shortUrl;
    }
}

