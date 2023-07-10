using UrlShortener.Application.Interfaces;
using UrlShortener.Application.Services;

namespace UrlShortener.Api.ConfigureServices;

public static class ConfigureApplicationServices
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddTransient<IAliasGenerator, AliasGenerator>();
        services.AddTransient<ILinkService, LinkService>();
        services.AddSingleton<ISystemDateTime>(new SystemDateTime());
    }
}
                                                                