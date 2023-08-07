using System.Security.Claims;
using UrlShortener.Entity;

namespace UrlShortener.Application.Interfaces;

public interface ITokenService
{
    string CreateAccessToken(IEnumerable<Claim> claims);
    string CreateRefreshToken();
    ClaimsPrincipal? GetClaimsFormToken(string token);
    IEnumerable<Claim> CreateUserClaims(User user,IEnumerable<string> roles);
}