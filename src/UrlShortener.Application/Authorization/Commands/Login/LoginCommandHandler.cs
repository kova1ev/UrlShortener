using System.Security.Claims;
using System.Security.Cryptography;
using MediatR;
using Microsoft.AspNetCore.Identity;
using UrlShortener.Application.Common.Constants;
using UrlShortener.Application.Common.Result;
using UrlShortener.Application.Interfaces;
using UrlShortener.Application.Responses;
using UrlShortener.Entity;

namespace UrlShortener.Application.Authorization.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<AuthResponse>>
{
    private readonly UserManager<User> _userManager;
    private readonly ITokenService _tokenService;
    private readonly ISystemDateTime _systemDateTime;

    public LoginCommandHandler(UserManager<User> userManager, ITokenService tokenService,
        ISystemDateTime systemDateTime)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _systemDateTime = systemDateTime;
    }

    public async Task<Result<AuthResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser == null)
        {
            return Result<AuthResponse>.Failure(new[] { AuthenticationErrorMessage.BadCredentials });
        }

        var isValidPassword = await _userManager.CheckPasswordAsync(existingUser, request.Password);
        if (!isValidPassword)
        {
            return Result<AuthResponse>.Failure(new[] { AuthenticationErrorMessage.BadCredentials });
        }

        var roles = await _userManager.GetRolesAsync(existingUser);
        var claims = _tokenService.CreateUserClaims(existingUser, roles);

        var accessToken = _tokenService.CreateAccessToken(claims);
        var refreshToken = _tokenService.CreateRefreshToken();

        existingUser.UpdateRefreshToken(refreshToken, _systemDateTime.UtcNow.AddDays(7));
        await _userManager.UpdateAsync(existingUser);

        return Result<AuthResponse>.Success(new AuthResponse()
            { AccessToken = accessToken, RefreshToken = refreshToken });
    }
}