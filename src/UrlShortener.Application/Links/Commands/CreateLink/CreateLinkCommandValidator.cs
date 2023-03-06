using FluentValidation;
using UrlShortener.Application.Common.Constants;

namespace UrlShortener.Application.Links.Commands.CreateLink;

public sealed class CreateLinkCommandValidator : AbstractValidator<CreateLinkCommand>
{
    public CreateLinkCommandValidator()
    {
        RuleFor(c => c.UrlAddress).NotEmpty().WithMessage(LinkValidationErrorMessage.URL_ADDRESS_REQUIRED);

        RuleFor(c => c.UrlAddress)
            .Must(url => url!.StartsWith("https://", StringComparison.OrdinalIgnoreCase)
                      || url.StartsWith("http://", StringComparison.OrdinalIgnoreCase)
                      || url.StartsWith("ftp://", StringComparison.OrdinalIgnoreCase))
            .When(c => c.UrlAddress != null)
            .WithMessage(LinkValidationErrorMessage.URL_ADDRESS_IS_NOT_URL);

        RuleFor(c => c.Alias)
            .Length(3, 30)
            .When(c => c.Alias != null)
            .WithMessage("{PropertyName} length must be in range {MinLength} - {MaxLength}");
    }
}


