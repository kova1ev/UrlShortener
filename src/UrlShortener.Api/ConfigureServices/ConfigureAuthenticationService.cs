using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using UrlShortener.Api.Authentication;
using UrlShortener.Api.Models;
using UrlShortener.Application.Common.Constants;

namespace UrlShortener.Api.ConfigureServices;

public static class ConfigureAuthenticationService
{
    public static void AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        JwtOptions jwtOptions = new();
        configuration.GetSection(JwtOptions.ConfigKey).Bind(jwtOptions);
        services.AddAuthentication(options =>
            {
                //options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwtOptions.Audience,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey))
                };
                options.Events = new JwtBearerEvents
                {
                    OnChallenge = async context =>
                    {
                        context.HandleResponse();
                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        context.Response.ContentType = "application/json";
                        var err = new ApiErrors(StatusCodes.Status401Unauthorized,
                            StatusCodeErrorMessage.JwtTokenErrorMessage);
                        var jsonSerializerOptions = new JsonSerializerOptions
                        {
                            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                        };
                        await context.Response.WriteAsJsonAsync(err, jsonSerializerOptions);
                    }
                };
            });
    }
}