using FluentValidation;
using UrlShortener.Application.Common.Constants;
using UrlShortener.Application.ValidationRules;

namespace UrlShortener.Application.Links.Commands.CreateLink;

public sealed class CreateLinkCommandValidator : AbstractValidator<CreateLinkCommand>
{
    public CreateLinkCommandValidator()
    {
        RuleFor(c => c.UrlAddress).NotEmpty().WithMessage(LinkValidationErrorMessage.URL_ADDRESS_REQUIRED);

        RuleFor(c => c.UrlAddress)
            .MustUrlAddress()
            .When(c => c.UrlAddress != null)
            .WithMessage(LinkValidationErrorMessage.URL_ADDRESS_IS_NOT_URL);

        RuleFor(c => c.Alias)
            .Length(3, 30)
            .When(c => c.Alias != null)
            .WithMessage(LinkValidationErrorMessage.ALIAS_BAD_RANGE);

        RuleFor(c => c.Alias)
            .Matches(@"^\S*$")
            .When(c => c.Alias != null)
            .WithMessage(LinkValidationErrorMessage.ALIAS_HAVE_WHITESPACE);
    }
}
