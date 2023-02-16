using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UrlShortener.Domain.Entity;

[Table("Links")]
public class Link : EntityBase
{
    public Link(string urlAddress, string shortName) : base()
    {
        ShortName = shortName ?? throw new ArgumentNullException(nameof(shortName));
        UrlAddress = urlAddress ?? throw new ArgumentNullException(nameof(urlAddress));

        //LinkInfo = linkInfo ?? throw new ArgumentNullException(nameof(linkInfo));
    }

    [Required]
    public string UrlAddress { get; set; }
    [Required]
    public string ShortName { get; set; }
    public DateTime DateTimeCreated { get; private set; } = DateTime.UtcNow;

    //[Required]
    //public LinkInfo LinkInfo { get; set; }

}


