using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Application.Dto.Link;

public class CreateLinkDto
{
    public CreateLinkDto(string urlAddress)
    {
        UrlAddress = urlAddress;
    }

    [Required]
    [Url]
    public string UrlAddress { get; set; }

    public string? Alias { get; set; }
}

