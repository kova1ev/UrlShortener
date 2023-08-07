namespace UrlShortener.Application.Common.Dto;

public class LinksRequestParameters : PageParameters
{
    public DateSort DateSort { get; set; }
    public string? Text { get; set; }

}
public enum DateSort
{
    Desc,
    Asc
}
