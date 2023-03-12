using Microsoft.AspNetCore.Mvc;
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
                    //options.Filters.Add(typeof(ApiKeyAuthorizeAttribute));
                });

            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                //todo
                options.SuppressModelStateInvalidFilter = true;
                //options.InvalidModelStateResponseFactory = (context) =>
                //{
                //    ApiError bad = new ApiError(400, "bedrequest", context.ModelState.SelectMany(k => k.Value.Errors.Select(e => e.ErrorMessage)));
                //    return new BadRequestObjectResult(bad);
                //};
            });

            builder.Services.Configure<AppOptions>(builder.Configuration.GetSection("AppOptions"));
            builder.Services.AddAppDbContext(builder.Configuration);
            builder.Services.AddApplication();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseWebAssemblyDebugging();
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                // app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseAppExceptionMiddleware();

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseAuthorization();

            // WASM 
            app.UseBlazorFrameworkFiles();
            app.MapFallbackToFile("index.html");

            app.MapControllers();

            app.Run();
        }

    }

}

