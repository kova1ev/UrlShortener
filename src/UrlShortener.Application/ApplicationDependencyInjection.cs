using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using UrlShortener.Application.Behaviors;
using UrlShortener.Application.Interfaces;
using UrlShortener.Application.Services;

namespace UrlShortener.Application;

public static class ApplicationDependencyInjection
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddTransient<IAliasGenerator, AliasGenerator>();
        services.AddTransient<ILinkService, LinkService>();
        services.AddSingleton<ISystemDateTime>(new SystemDateTime());

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
    }
}