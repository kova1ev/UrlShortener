using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Application.Commands.Links;

public class UpdateLinkDto
{

    [Required]
    [MaxLength(10)]
    [MinLength(3)]
    public string? Alias { get; set; }

    [Required]
    [Url]
    [MinLength(10)]
    public string? UrlAddress { get; set; }

    public UpdateLinkDto(string? alias, string? urlAddress)
    {
        Alias = alias;
        UrlAddress = urlAddress;
    }
}
