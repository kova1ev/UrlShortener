namespace UrlShortener.Application.Responses;

public sealed class LinkCreatedResponse
{
    public string ShortUrl { get; set; }
    public Guid Id { get; set; }
    public LinkCreatedResponse(Guid id, string shortUrl)
    {
        Id = id;
        ShortUrl = shortUrl;
    }
}
