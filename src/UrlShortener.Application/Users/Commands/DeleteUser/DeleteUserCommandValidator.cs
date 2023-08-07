using FluentValidation;
using UrlShortener.Application.Common.Constants;

namespace UrlShortener.Application.Users.Commands.DeleteUser;

public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty().WithMessage(UserErrorMessage.UserIdIsRequired);
    }
}