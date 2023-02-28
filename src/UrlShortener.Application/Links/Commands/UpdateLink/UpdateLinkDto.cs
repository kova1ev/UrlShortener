using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Application.Links.Commands.UpdateLink;

public class UpdateLinkDto
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(10)]
    [MinLength(3)]
    public string? Alias { get; set; }

    [Required]
    [Url]
    [MinLength(10)]
    public string? UrlAddress { get; set; }

    public UpdateLinkDto(string? alias, string? urlAddress, Guid id)
    {
        Alias = alias;
        UrlAddress = urlAddress;
        Id = id;
    }
}
