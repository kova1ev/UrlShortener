using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using UrlShortener.Api.Authentication;
using UrlShortener.Application.Interfaces;
using UrlShortener.Entity;

namespace UrlShortener.Api.Infrastructure;

public class JwtTokenService : ITokenService
{
    private readonly JwtOptions _jwtOptions;
    private readonly JwtSecurityTokenHandler _tokenHandler = new();
    private readonly ILogger<JwtTokenService> _logger;

    public JwtTokenService(IOptions<JwtOptions> options, ILogger<JwtTokenService> logger)
    {
        _logger = logger;
        _jwtOptions = options.Value;
    }

    public string CreateAccessToken(IEnumerable<Claim> claims)
    {
        var time = DateTime.Now;
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));

        var jwt = new JwtSecurityToken(
            _jwtOptions.Issuer,
            _jwtOptions.Audience,
            claims,
            expires: time.AddMinutes(_jwtOptions.LifeTime),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

        var jwtToken = _tokenHandler.WriteToken(jwt);

        return jwtToken;
    }

    public string CreateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public ClaimsPrincipal? GetClaimsFormToken(string token)
    {
        var validateParameters = new TokenValidationParameters
        {
            ValidateLifetime = false,

            ValidateIssuer = true,
            ValidIssuer = _jwtOptions.Issuer,

            ValidateAudience = true,
            ValidAudience = _jwtOptions.Audience,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = SymmetricSecurityKey
        };

        ClaimsPrincipal? claimsPrincipal = null;
        try
        {
            claimsPrincipal = _tokenHandler.ValidateToken(token, validateParameters, out var securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token or SecurityAlgorithms.");
            }
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Invalid Token: {@Message}", exception.Message);
        }
        
        return claimsPrincipal;
    }
    
    public  IEnumerable<Claim> CreateUserClaims(User user, IEnumerable<string> roles)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.UserName!),
            new(ClaimTypes.Email, user.Email!),
        };
        claims.AddRange(roles.Select(item => new Claim(ClaimTypes.Role, item)));
        return claims;
    }


    private SymmetricSecurityKey SymmetricSecurityKey =>
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));
}