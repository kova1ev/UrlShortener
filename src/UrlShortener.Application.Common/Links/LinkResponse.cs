namespace UrlShortener.Application.Common.Links;

public sealed class LinkResponse
{
    public string ShortUrl { get; set; }
    public Guid Id { get; set; }
    public LinkResponse(Guid id, string shortUrl)
    {
        Id = id;
        ShortUrl = shortUrl;
    }
}
