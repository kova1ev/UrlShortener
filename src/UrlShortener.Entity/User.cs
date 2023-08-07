using Microsoft.AspNetCore.Identity;

namespace UrlShortener.Entity;

public class User : IdentityUser<Guid>
{
    public string? RefreshToken { get; private set; }
    public DateTime? RefreshTokenExpiryTime { get; private set; }

    public IEnumerable<Link>? Links { get; set; }

    public void UpdateRefreshToken(string token, DateTime dateTime)
    {
        RefreshToken = token;
        RefreshTokenExpiryTime = dateTime;
    }

    public void RevokeRefreshToken()
    {
        RefreshToken = null;
        RefreshTokenExpiryTime = null;
    }
}