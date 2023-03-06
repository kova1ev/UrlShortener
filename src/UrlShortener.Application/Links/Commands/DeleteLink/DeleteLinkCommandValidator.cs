using FluentValidation;
using UrlShortener.Application.Common.Constants;

namespace UrlShortener.Application.Links.Commands.DeleteLink;

public sealed class DeleteLinkCommandValidator : AbstractValidator<DeleteLinkCommand>
{
    public DeleteLinkCommandValidator()
    {
        RuleFor(command => command.Id).NotEmpty().WithMessage(LinkValidationErrorMessage.ID_REQUIRED);
    }
}
