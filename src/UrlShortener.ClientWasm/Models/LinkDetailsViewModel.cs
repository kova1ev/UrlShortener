namespace UrlShortener.ClientWasm.Models;

public class LinkDetailsViewModel
{
    public Guid Id { get; set; }
    public string? UrlAddress { get; set; }
    public string? UrlShort { get; set; }
    public DateTime DateTimeCreated { get; set; }

    public LinkStatisticViewModel LinkStatistic { get; set; } //= new LinkStatisticViewModel();
    public LinkDetailsViewModel() { }
}

public class LinkStatisticViewModel
{
    public Guid Id { get; set; }
    public string? DomainName { get; set; }
    public DateTime? LastUse { get; set; }
    public int Clicks { get; set; }
    public string? Browser { get; set; }
    public string? Os { get; set; }
    public string? Country { get; set; }
    public string? Region { get; set; }
    public string? City { get; set; }
}


