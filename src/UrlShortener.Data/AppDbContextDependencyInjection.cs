using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UrlShortener.Application.Interfaces;

namespace UrlShortener.Data;

public static class AppDbContextDependencyInjection
{
    public static void AddAppDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration["ConnectionStrings:PostgresSQL"]
            ?? throw new ArgumentNullException(nameof(configuration));

        services.AddScoped<IAppDbContext>(sp => sp.GetRequiredService<AppDbContext>());

        //services.AddDbContext<AppDbContext>(options =>
        //{
        //    options.UseNpgsql(connectionString);
        //});

        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseInMemoryDatabase("test");

        });
    }
}
