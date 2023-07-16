using FluentValidation;
using UrlShortener.Application;

namespace UrlShortener.Api.ConfigureServices;

public static class ConfigureFluentValidator
{
    public static void AddFluentValidationService(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(ApplicationAssembly.Assembly);
    }
}
