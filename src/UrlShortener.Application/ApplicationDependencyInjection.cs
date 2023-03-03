using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using UrlShortener.Application.Behaviors;
using UrlShortener.Application.Interfaces;
using UrlShortener.Application.Links.Commands.CreateLink;
using UrlShortener.Application.Links.Commands.DeleteLink;
using UrlShortener.Application.Links.Commands.UpdateLink;
using UrlShortener.Application.Services;

namespace UrlShortener.Application;

public static class ApplicationDependencyInjection
{

    public static void AddApplication(this IServiceCollection services)
    {
        //var assembly = AppDomain.CurrentDomain.Load("Application");
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        services.AddMediatR(assemblies);
        // services.AddMediatR(typeof(AssemblyReference).Assembly);

        services.AddTransient<IAliasCteater, AliasCreater>();
        services.AddTransient<IAliasService, AliasService>();

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        //services.AddFluentValidationAutoValidation();
        //  services.AddFluentValidationAutoValidation(x => x.DisableDataAnnotationsValidation = true);
        services.AddScoped<IValidator<CreateLinkCommand>, CreateLinkCommandValidator>();
        services.AddScoped<IValidator<DeleteLinkCommand>, DeleteLinkCommandValidator>();
        services.AddScoped<IValidator<UpdateLinkCommand>, UpdateLinkCommandValidator>();

        //services.AddValidatorsFromAssembly(typeof(Application.AssemblyReference).Assembly);
        //services.AddValidatorsFromAssembly(typeof(AssemblyReference).Assembly);
        //  services.AddValidatorsFromAssembly(typeof(AssemblyReference).Assembly);


    }
}
