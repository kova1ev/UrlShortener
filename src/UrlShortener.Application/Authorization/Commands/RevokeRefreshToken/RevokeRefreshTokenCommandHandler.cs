using MediatR;
using Microsoft.AspNetCore.Identity;
using UrlShortener.Application.Common.Constants;
using UrlShortener.Application.Common.Result;
using UrlShortener.Entity;

namespace UrlShortener.Application.Authorization.Commands.RevokeRefreshToken;

public class RevokeRefreshTokenCommandHandler : IRequestHandler<RevokeRefreshTokenCommand, Result>
{
    private readonly UserManager<User> _userManager;

    public RevokeRefreshTokenCommandHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result> Handle(RevokeRefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.Id.ToString());
        if (user == null)
        {
            return Result.Failure(new[] { UserErrorMessage.UserNotFound });
        }

        user.RevokeRefreshToken();
        await _userManager.UpdateAsync(user);
        return Result.Success();
    }
}