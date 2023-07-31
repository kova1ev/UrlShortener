using Microsoft.AspNetCore.Identity;

namespace UrlShortener.Entity;

public class User : IdentityUser<Guid>
{
    public IEnumerable<Link>? Links { get; set; }
}