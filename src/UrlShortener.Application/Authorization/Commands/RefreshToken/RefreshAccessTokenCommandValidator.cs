using FluentValidation;
using UrlShortener.Application.Common.Constants;

namespace UrlShortener.Application.Authorization.Commands.RefreshToken;

public class RefreshAccessTokenCommandValidator : AbstractValidator<RefreshAccessTokenCommand>
{
    public RefreshAccessTokenCommandValidator()
    {
        RuleFor(command => command.AccessToken).NotEmpty().WithMessage(TokenErrorMessage.AccessTokenIsRequired);
        RuleFor(command => command.RefreshToken).NotEmpty().WithMessage(TokenErrorMessage.RefreshTokenIsRequired);
    }
}