namespace UrlShortener.Application.Commands.Links;

public class UpdateLinkDto
{
    public string? Alias { get; set; }

    public string? UrlAddress { get; set; }

    public UpdateLinkDto(string? alias, string? urlAddress)
    {
        Alias = alias;
        UrlAddress = urlAddress;
    }
}
