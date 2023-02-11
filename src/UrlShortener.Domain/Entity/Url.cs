using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Domain.Entity;

public class Url : EntityBase
{
    public Url(string urlAddress, string urlShort) : base()
    {
        UrlAddress = urlAddress ?? throw new ArgumentNullException(nameof(urlAddress));
        UrlShort = urlShort ?? throw new ArgumentNullException(nameof(urlShort));
    }

    [Required]
    public string UrlAddress { get; set; }
    [Required]
    public string UrlShort { get; set; }


}
