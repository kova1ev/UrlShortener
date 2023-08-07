using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Identity;
using UrlShortener.Application.Common.Constants;
using UrlShortener.Application.Common.Result;
using UrlShortener.Application.Interfaces;
using UrlShortener.Application.Responses;
using UrlShortener.Entity;

namespace UrlShortener.Application.Authorization.Commands.RefreshToken;

public class RefreshAccessTokenCommandHandler : IRequestHandler<RefreshAccessTokenCommand, Result<AuthResponse>>
{
    private readonly UserManager<User> _userManager;
    private readonly ITokenService _tokenService;
    private readonly ISystemDateTime _systemDateTime;

    public RefreshAccessTokenCommandHandler(UserManager<User> userManager, ITokenService tokenService,
        ISystemDateTime systemDateTime)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _systemDateTime = systemDateTime;
    }

    public async Task<Result<AuthResponse>> Handle(RefreshAccessTokenCommand request,
        CancellationToken cancellationToken)
    {
        var claimsPrincipal = _tokenService.GetClaimsFormToken(request.AccessToken);
        if (claimsPrincipal == null)
        {
            return Result<AuthResponse>.Failure(new[] { TokenErrorMessage.InvalidAccessToken });
        }

        var userId = claimsPrincipal.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null || user.RefreshToken != request.RefreshToken ||
            user.RefreshTokenExpiryTime <= _systemDateTime.UtcNow)
        {
            return Result<AuthResponse>.Failure(new[] { TokenErrorMessage.InvalidRefreshToken });
        }

        var roles = await _userManager.GetRolesAsync(user);
        var claims = _tokenService.CreateUserClaims(user, roles);
        var accessToken = _tokenService.CreateAccessToken(claims);
        var refreshToken = _tokenService.CreateRefreshToken();

        user.UpdateRefreshToken(refreshToken, _systemDateTime.UtcNow.AddDays(7));
        await _userManager.UpdateAsync(user);


        return Result<AuthResponse>.Success(new AuthResponse()
            { RefreshToken = refreshToken, AccessToken = accessToken });
    }
}