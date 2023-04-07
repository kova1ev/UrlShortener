using System.ComponentModel.DataAnnotations;

namespace UrlShortener.ClientWasm.Models;

public class CreateLinkModel
{
    [Required]
    [Url]
    public string UrlAddress { get; set; }

    [StringLength(30, MinimumLength = 3)]
    public string? Alias { get; set; }

}