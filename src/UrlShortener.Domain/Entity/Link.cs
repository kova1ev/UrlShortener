using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UrlShortener.Domain.Entity;

[Table("links")]
public class Link : EntityBase
{
    //public Link(s) : base()
    //{
    //    ShortName = shortName ?? throw new ArgumentNullException(nameof(shortName));
    //    UrlAddress = urlAddress ?? throw new ArgumentNullException(nameof(urlAddress));
    //}

    [Required]
    public string UrlAddress { get; set; }
    [Required]
    public string ShortName { get; set; }
    public DateTime DateTimeCreated { get; private set; } = DateTime.UtcNow;

    [Required]
    public LinkInfo LinkInfo { get; set; }

}


