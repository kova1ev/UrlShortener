using Microsoft.AspNetCore.Identity;
using UrlShortener.Data;
using UrlShortener.Entity;

namespace UrlShortener.Api.ConfigureServices;

public static class ConfigureIdentity
{
    public static void AddIdentity(this IServiceCollection service)
    {
        service.AddIdentity<User, IdentityRole<Guid>>(options =>
        {
            options.User.RequireUniqueEmail = true;
            options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 8;
        }).AddEntityFrameworkStores<AppDbContext>();
    }
}