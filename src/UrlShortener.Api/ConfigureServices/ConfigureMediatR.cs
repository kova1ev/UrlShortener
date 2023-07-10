using MediatR;
using UrlShortener.Application;
using UrlShortener.Application.Behaviors;

namespace UrlShortener.Api.ConfigureServices;

public static class ConfigureMediatR
{
    public static void AddMediatRServices(this IServiceCollection services)
    {
        services.AddMediatR(ApplicationAssembly.Assembly);
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
    }
}