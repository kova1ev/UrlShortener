using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using UrlShortener.Api.Authentication;
using UrlShortener.Api.ConfigureServices;
using UrlShortener.Api.Filters;
using UrlShortener.Api.Infrastructure;
using UrlShortener.Api.Middleware;
using UrlShortener.Application.Common;
using UrlShortener.Application.Common.Domain;
using UrlShortener.Application.Interfaces;
using UrlShortener.Data;
using UrlShortener.Entity;

namespace UrlShortener.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        // Add services to the container.

        builder.Services.AddControllers();
        builder.Services.AddRazorPages();
        builder.Services.AddHttpContextAccessor();
        builder.Services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });
        builder.Services.Configure<JsonOptions>(options =>
        {
            options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        });
        builder.Services.Configure<MvcOptions>(options =>
        {
            options.Filters.Add(new ProducesAttribute("application/json"));
            options.Filters.Add(typeof(ValidateModelStateFilter));
            options.RespectBrowserAcceptHeader = true;
            options.ReturnHttpNotAcceptable = true;
        });

        builder.Services.Configure<AppOptions>(builder.Configuration.GetSection(AppOptions.ConfigKey));
        builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(JwtOptions.ConfigKey));

        builder.Services.AddScoped<ITokenProvider, JwtTokenProvider>();
        builder.Services.AddHttpClient<IGeolocationService, GeolocationService>();

        builder.Services.AddAppDbContext(builder.Configuration);
        builder.Services.AddApplicationServices();
        builder.Services.AddFluentValidationService();
        builder.Services.AddMediatRServices();

        builder.Services.AddIdentity();
        builder.Services.AddAuthentication(builder.Configuration);


        builder.Services.AddSwagger();
        builder.AddSerilog();


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

        // app.UseHsts();
        app.UseAppExceptionMiddleware();
        //app.UseHttpsRedirection();

        // WASM
        // app.UseBlazorFrameworkFiles();
        app.UseStaticFiles();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
        app.MapRazorPages();

        // WASM 
        // app.MapFallbackToFile("index.html");


        SeedData.SeedIdentityAsync(app).ConfigureAwait(false);
        
        app.Run();
    }
}