namespace UrlShortener.Application.Common.Domain.Links;

public class UpdateLinkRequest
{
    public string? UrlAddress { get; set; }
    public string? Alias { get; set; }
}