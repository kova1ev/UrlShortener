using MediatR;
using Microsoft.Extensions.DependencyInjection;
using UrlShortener.Application.Interfaces;
using UrlShortener.Application.Services;

namespace UrlShortener.Application;

public static class ApplicationDependencyInjection
{

    public static void AddApplication(this IServiceCollection services)
    {
        //var assembly = AppDomain.CurrentDomain.Load("Application");
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        services.AddMediatR(assemblies);

        services.AddTransient<IAliasCteater, AliasCreater>();
        services.AddTransient<IAliasService, AliasService>();
    }
}
