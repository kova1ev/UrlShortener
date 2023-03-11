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
    // TODO : clear! 
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddTransient<IAliasGenerator, AliasGenerator>();
        services.AddTransient<ILinkService, LinkService>();

        //var assembly = AppDomain.CurrentDomain.Load("Application");
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        //services.AddMediatR(assemblies);

        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        // services.AddScoped<IValidator<CreateLinkCommand>, CreateLinkCommandValidator>();
        // services.AddScoped<IValidator<DeleteLinkCommand>, DeleteLinkCommandValidator>();
        // services.AddScoped<IValidator<UpdateLinkCommand>, UpdateLinkCommandValidator>();

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
