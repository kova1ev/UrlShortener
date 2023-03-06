namespace UrlShortener.Domain.Entity;


public class Link : EntityBase
{

    public string UrlAddress { get; set; }

    public string Alias { get; set; }

    public string UrlShort { get; set; }

    public DateTime DateTimeCreated { get; private set; } = DateTime.UtcNow;



    public LinkInfo LinkInfo { get; set; }
}


