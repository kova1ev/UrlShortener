using FluentValidation;
using UrlShortener.Application.Common.Constants;
using UrlShortener.Application.ValidationRules;

namespace UrlShortener.Application.Links.Commands.CreateLink;

public sealed class CreateLinkCommandValidator : AbstractValidator<CreateLinkCommand>
{
    public CreateLinkCommandValidator()
    {
        RuleFor(c => c.UrlAddress).NotEmpty().WithMessage(LinkValidationErrorMessage.UrlAddressRequired);

        RuleFor(c => c.UrlAddress)
            .MustUrlAddress()
            .When(c => c.UrlAddress != null)
            .WithMessage(LinkValidationErrorMessage.UrlAddressIsNotUrl);

        RuleFor(c => c.Alias)
            .Length(3, 30)
            .When(c => c.Alias != null)
            .WithMessage(LinkValidationErrorMessage.AliasBadRange);

        RuleFor(c => c.Alias)
            .Matches(@"^\S*$")
            .When(c => c.Alias != null)
            .WithMessage(LinkValidationErrorMessage.AliasHaveWhitespace);
    }
}
