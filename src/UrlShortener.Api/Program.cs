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

            builder.Services.AddControllers()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm";
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                //  options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            })
            .ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
                // options.SuppressMapClientErrors = true;
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
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseMiddleware<Mymiddleware>();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}


public class Mymiddleware
{
    RequestDelegate next;

    public Mymiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        await next.Invoke(context);

        var s = context.Response;
        var statusCode = context.Response.StatusCode;
        var c = context.Response.Body;

    }
}