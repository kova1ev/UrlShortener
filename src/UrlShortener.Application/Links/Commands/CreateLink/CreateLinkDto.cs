using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Application.Links.Commands.CreateLink;

public class CreateLinkDto
{
    [Required]
    [Url]
    [MinLength(10)]
    public string UrlAddress { get; set; }

    [MinLength(3)]
    [MaxLength(10)]
    public string? Alias { get; set; }
}

