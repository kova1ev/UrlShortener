using FluentValidation;

namespace UrlShortener.Application.Links.Commands.CreateLink;

public sealed class CreateLinkCommandValidator : AbstractValidator<CreateLinkCommand>
{
    public CreateLinkCommandValidator()
    {

        RuleFor(c => c.UrlAddress).NotEmpty().WithMessage("{PropertyName} is required.");

        RuleFor(c => c.UrlAddress)
            .Must(url => url.StartsWith("https://", StringComparison.OrdinalIgnoreCase)
                      || url.StartsWith("http://", StringComparison.OrdinalIgnoreCase)
                      || url.StartsWith("ftp://", StringComparison.OrdinalIgnoreCase))
            .When(c => c.UrlAddress != null)
            .WithMessage("{PropertyName} is not url.");

        RuleFor(c => c.Alias)
            .Length(3, 50)
            .When(c => c.Alias != null)
            .WithMessage("{PropertyName} length must be in range {MinLength} - {MaxLength}");
    }
}


