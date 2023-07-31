using Microsoft.EntityFrameworkCore;
using UrlShortener.Application.Interfaces;
using UrlShortener.Data;

namespace UrlShortener.Api.ConfigureServices;

public static class ConfigureAppDbContext
{
    public static void AddAppDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        bool inMemory = configuration.GetValue<bool>("InMemory");
        if (inMemory)
        {
            services.AddDbContext<AppDbContext>(options => { options.UseInMemoryDatabase("test"); });
        }
        else
        {
            string connectionString = configuration["ConnectionStrings:PostgresSQL"]
                                      ?? throw new ArgumentNullException(nameof(configuration));
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(connectionString).UseSnakeCaseNamingConvention();
            });
        }

        services.AddScoped<IAppDbContext>(sp => sp.GetRequiredService<AppDbContext>());
    }
}