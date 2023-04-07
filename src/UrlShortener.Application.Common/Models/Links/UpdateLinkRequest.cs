namespace UrlShortener.Application.Common.Models.Links;

public class UpdateLinkRequest
{
    public string? UrlAddress { get; set; }
    public string? Alias { get; set; }
}
