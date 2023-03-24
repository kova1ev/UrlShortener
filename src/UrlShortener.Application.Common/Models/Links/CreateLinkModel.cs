using System.ComponentModel.DataAnnotations;
using UrlShortener.Application.Common.Constants;
using UrlShortener.Application.Common.Utility;

namespace UrlShortener.Application.Common.Models.Links;

public class CreateLinkModel : IValidatableObject
{
    public string? UrlAddress { get; set; }
    public string? Alias { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        Alias = Alias.TrimAndSetNull();
        UrlAddress = UrlAddress.TrimAndSetNull();
        if (UrlAddress == null)
            yield return new ValidationResult(LinkValidationErrorMessage.URL_ADDRESS_REQUIRED);
    }
}

