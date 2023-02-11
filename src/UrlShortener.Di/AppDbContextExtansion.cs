using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UrlShortener.Application.Interfaces;
using UrlShortener.Data;

namespace UrlShortener.Di;

public static class AppDbContextExtansion
{

    public static void AddAppDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IAppDbContext>(provider => provider.GetRequiredService<AppDbContext>());

        string connectionString = configuration["ConnectionStrings:MsSql"]
            ?? throw new ArgumentNullException($"{nameof(configuration)} does not contain connectionStrings");

        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });
    }
}
