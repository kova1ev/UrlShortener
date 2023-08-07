using MediatR;
using UrlShortener.Application.Common.Result;

namespace UrlShortener.Application.Authorization.Commands.RevokeRefreshToken;

public class RevokeRefreshTokenCommand : IRequest<Result>
{
    public RevokeRefreshTokenCommand(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; }
}