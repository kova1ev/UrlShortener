namespace UrlShortener.Application.Commands.Links;

public class CreateLinkDto
{
    //[Required]
    // [Url]
    //[MinLength(10)]
    public string UrlAddress { get; set; }

    //[MinLength(3)]
    //[MaxLength(10)]
    public string? Alias { get; set; }
}

