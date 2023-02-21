using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Application.Dto.Link;

public class CreateLinkDto
{
    public CreateLinkDto(string urlAddress, string? alias)
    {
        UrlAddress = urlAddress;
        Alias = alias;
    }

    [Required]
    [Url]
    [MinLength(10)]
    public string UrlAddress { get; }
    [MinLength(3)]
    [MaxLength(10)]
    public string? Alias { get; }
}

