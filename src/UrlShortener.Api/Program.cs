using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using UrlShortener.Api.Filters;
using UrlShortener.Api.Middleware;
using UrlShortener.Application;
using UrlShortener.Application.Common;
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

            builder.Services.Configure<JsonOptions>(options =>
            {
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            });

            builder.Services.Configure<MvcOptions>(options =>
                {
                    options.Filters.Add(typeof(ValidateModelStateFilter));
                });

            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            builder.Services.Configure<AppOptions>(builder.Configuration.GetSection("AppOptions"));
            builder.Services.AddAppDbContext(builder.Configuration);
            builder.Services.AddApplication();

            builder.Services.AddRazorPages();

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
                    var scheme = new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "ApiKey"
                        },
                        In = ParameterLocation.Header
                    };
                    var requirement = new OpenApiSecurityRequirement
                    {
                        { scheme,new List<string>()}
                    };
                    options.AddSecurityRequirement(requirement);
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
            app.UseBlazorFrameworkFiles();

            app.UseStaticFiles();
            app.UseAuthorization();
            app.MapControllers();
            app.MapRazorPages();
            // WASM 
            app.MapFallbackToFile("index.html");

            app.Run();
        }







    }

}

