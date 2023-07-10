using Microsoft.OpenApi.Models;

namespace UrlShortener.Api.ConfigureServices;

public static class ConfigureSwagger
{
    public static void AddSwagger(this IServiceCollection services)
    {
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "UrlShortener Api", Version = "v1" });
            options.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
            {
                Description = "ApiKey authorization",
                Type = SecuritySchemeType.ApiKey,
                Name = "x-api-key",
                In = ParameterLocation.Header,
                Scheme = "ApiKeyScheme"
            });
            var apiKey = new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                },
                In = ParameterLocation.Header
            };
            var apiKEyRequirement = new OpenApiSecurityRequirement
            {
                { apiKey, new List<string>() }
            };
            options.AddSecurityRequirement(apiKEyRequirement);

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                In = ParameterLocation.Header,
                Scheme = "Bearer",
                BearerFormat = "JWT"
            });
            var bearer = new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                In = ParameterLocation.Header
            };
            var bearerRequirement = new OpenApiSecurityRequirement
            {
                { bearer, new List<string>() }
            };
            options.AddSecurityRequirement(bearerRequirement);
        });
    }

}