using FluentValidation;
using UrlShortener.Application.Common.Constants;

namespace UrlShortener.Application.Users.Commands.CreateUser;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(c => c.UserName).NotEmpty().WithMessage(UserErrorMessage.UserNameIsRequired);
        RuleFor(c => c.UserName)
            .Length(3, 50)
            .When(c => c.UserName != null!)
            .WithMessage(UserErrorMessage.UserNameIsBadRange);

        RuleFor(c => c.Email).NotEmpty().WithMessage(UserErrorMessage.UserEmailIsRequired);
        RuleFor(c => c.Email).EmailAddress().WithMessage(UserErrorMessage.InvalidUserEmail);

        RuleFor(c => c.Password).NotEmpty().WithMessage(UserErrorMessage.UserPasswordIsRequired);
    }
}