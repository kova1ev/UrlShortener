using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json.Serialization;

namespace UrlShortener.ClientWasm.Models;

public class JwtToken
{
    [JsonPropertyName("token")]
    public string? Value { get; set; }


    public static IEnumerable<Claim> ExtractClaims(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var decodeToken = handler.ReadJwtToken(token);

        return decodeToken.Claims;
    }
}
