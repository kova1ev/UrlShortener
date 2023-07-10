using Serilog;

namespace UrlShortener.Api.ConfigureServices;

public static class ConfigureSerilog
{
    public static void AddSerilog(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, config) =>
        {
            config.ReadFrom.Configuration(builder.Configuration)
#if DEBUG
                .WriteTo.Seq("http://localhost:5341")
#endif
                .Enrich.FromLogContext();
        });
    }
}