using System.Security.Claims;
using UrlShortener.Entity;

namespace UrlShortener.Application.Interfaces;

public interface ITokenProvider
{
    string CreateToken(IEnumerable<Claim> claims);
}