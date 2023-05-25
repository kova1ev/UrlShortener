using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using UrlShortener.Api.Authentication;
using UrlShortener.Api.Filters;
using UrlShortener.Api.Infrastructure;
using UrlShortener.Api.Middleware;
using UrlShortener.Api.Models;
using UrlShortener.Application;
using UrlShortener.Application.Common;
using UrlShortener.Application.Common.Models;
using UrlShortener.Data;

namespace UrlShortener.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddRazorPages();
            builder.Services.Configure<AppOptions>(builder.Configuration.GetSection(AppOptions.CONFIG_KEY));
            builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(JwtOptions.ConfigKey));
            builder.Services.Configure<User>(builder.Configuration.GetSection("Admin"));
            builder.Services.AddScoped<TokenProvider, TokenProvider>();

            JwtOptions jwtOptions = new();
            ConfigurationBinder.Bind(builder.Configuration.GetSection(JwtOptions.ConfigKey), jwtOptions);
            builder.Services.Configure<JsonOptions>(options =>
            {
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            });

            builder.Services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(typeof(ValidateModelStateFilter));
                options.RespectBrowserAcceptHeader = true;
                options.ReturnHttpNotAcceptable = true;
            });

            builder.Services.AddLogging(options =>
            {
                options.AddSimpleConsole(c =>
                {
                    c.TimestampFormat = "[yyyy-MM-dd HH:mm:ss] ";
                });
            });

            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            builder.Services.AddAuthentication(options =>
            {
                //options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
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
                                   ApiErrors err = new ApiErrors(StatusCodes.Status401Unauthorized, "JwtToken is required", null);
                                   JsonSerializerOptions options = new JsonSerializerOptions()
                                   {
                                       DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                                   };
                                   await context.Response.WriteAsJsonAsync(err, options);
                               }
                    };
                });
            builder.Services.AddAuthorization();



            builder.Services.AddAppDbContext(builder.Configuration);
            builder.Services.AddApplication();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
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
                        { apiKey,new List<string>()}
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
                        { bearer,new List<string>()}
                    };
                    options.AddSecurityRequirement(bearerRequirement);
                });


            builder.Services.AddHttpClient<IGeolocationService, GeolocationService>();

            builder.Host.UseSerilog((context, config) =>
            {
                config.ReadFrom.Configuration(builder.Configuration)
#if DEBUG
                       .WriteTo.Seq("http://localhost:5341")
#endif
                      .Enrich.FromLogContext();
            });



            var app = builder.Build();


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseWebAssemblyDebugging();
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "UrlShortener API V1");
                });
            }
            else
            {
                // app.UseHsts();
            }

            app.UseAppExceptionMiddleware();
            //app.UseHttpsRedirection();

            // WASM
            // app.UseBlazorFrameworkFiles();

            // app.UseStaticFiles();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.MapRazorPages();

            // WASM 
            //app.MapFallbackToFile("index.html");

            app.Run();
        }

    }

}

