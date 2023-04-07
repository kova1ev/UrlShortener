using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UrlShortener.Api.Authentication;
using UrlShortener.Application.Common.Models;

namespace UrlShortener.Api.Infrastructure;

public interface ITokenProvider
{
    string CreateToken(User user);
}

public class TokenProvider : ITokenProvider
{
    private readonly JwtOptions _jwtOptions;

    public TokenProvider(IOptions<JwtOptions> options)
    {
        _jwtOptions = options.Value;
    }

    public string CreateToken(User user)
    {

        IEnumerable<Claim> claims = GetClaims(user);

        DateTime time = DateTime.UtcNow;
        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));

        JwtSecurityToken jwt = new JwtSecurityToken(
           issuer: _jwtOptions.Issuer,
           audience: _jwtOptions.Audience,
           claims: claims,
           notBefore: time,
           expires: time.AddMinutes(_jwtOptions.LifeTime),
           signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

        var jwtToken = new JwtSecurityTokenHandler().WriteToken(jwt);

        return jwtToken;
    }


    private IEnumerable<Claim> GetClaims(User user)
    {
        string name = user.Email.Substring(0, user.Email.IndexOf('@'));
        IEnumerable<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,name),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Role,user.Role)
            };
        return claims;
    }
}
