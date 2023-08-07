using FluentValidation;
using UrlShortener.Application.Common.Constants;

namespace UrlShortener.Application.Authorization.Commands.RevokeRefreshToken;

public class RevokeRefreshTokenCommandValidator : AbstractValidator<RevokeRefreshTokenCommand>
{
    public RevokeRefreshTokenCommandValidator()
    {
        RuleFor(command => command.Id).NotEmpty().WithMessage(UserErrorMessage.UserIdIsRequired);
    }
}