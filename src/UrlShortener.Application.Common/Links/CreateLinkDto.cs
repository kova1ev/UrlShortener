namespace UrlShortener.Application.Common.Links;

public class CreateLinkDto
{
    public CreateLinkDto(string urlAddress, string? alias)
    {
        UrlAddress = urlAddress;
        Alias = alias;
    }

    public string UrlAddress { get; set; }

    public string? Alias { get; set; }

}

