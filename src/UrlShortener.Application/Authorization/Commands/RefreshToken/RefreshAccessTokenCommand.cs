using MediatR;
using UrlShortener.Application.Common.Result;
using UrlShortener.Application.Responses;

namespace UrlShortener.Application.Authorization.Commands.RefreshToken;

public class RefreshAccessTokenCommand : IRequest<Result<AuthResponse>>
{
    public RefreshAccessTokenCommand(string refreshToken, string accessToken)
    {
        RefreshToken = refreshToken;
        AccessToken = accessToken;
    }

    public string AccessToken { get; }

    public string RefreshToken { get; }
}