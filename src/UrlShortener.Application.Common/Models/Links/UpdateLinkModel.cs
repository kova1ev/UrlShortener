using System.ComponentModel.DataAnnotations;
using UrlShortener.Application.Common.Utility;

namespace UrlShortener.Application.Common.Models.Links;

public class UpdateLinkModel : IValidatableObject
{
    public string? UrlAddress { get; set; }
    public string? Alias { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        UrlAddress = UrlAddress.TrimAndSetNull();
        Alias = Alias.TrimAndSetNull();
        return Enumerable.Empty<ValidationResult>();
    }
}
