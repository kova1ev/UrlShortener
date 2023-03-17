using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace UrlShortener.Data;

public static class AppDbContextDependencyInjection
{
    public static void AddAppDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        //string connectionString = configuration["ConnectionStrings:PostgresSQL"]
        //    ?? throw new ArgumentNullException(nameof(configuration));

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
