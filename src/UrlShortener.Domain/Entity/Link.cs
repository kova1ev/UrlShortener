using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UrlShortener.Domain.Entity;

[Table("links")]
public class Link : EntityBase
{
    [Required]
    public string UrlAddress { get; set; }
    [Required]
    public string Alias { get; set; }
    [Required]
    public string UrlShort { get; set; }
    [Required]
    public DateTime DateTimeCreated { get; private set; } = DateTime.UtcNow;


    public LinkInfo LinkInfo { get; set; }
}


