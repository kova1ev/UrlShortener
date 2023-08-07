using FluentValidation;
using UrlShortener.Application.Common.Constants;

namespace UrlShortener.Application.Authorization.Commands.Login;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(l => l.Email)
            .EmailAddress()
            .NotEmpty()
            .WithMessage(UserErrorMessage.InvalidUserEmail);

        RuleFor(l => l.Password).NotEmpty().WithMessage(UserErrorMessage.UserPasswordIsRequired);
    }
}