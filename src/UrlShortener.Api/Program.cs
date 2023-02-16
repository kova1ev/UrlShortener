using UrlShortener.Application;
using UrlShortener.Di;

namespace UrlShortener.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Add services to the container.

            builder.Services.AddControllers();


            builder.Services.AddAppDbContext(builder.Configuration);

            builder.Services.AddApplication();
            //builder.Services.AddMediatR(Assembly.GetEntryAssembly());


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

            //app.UseHttpsRedirection();

            //app.UseAuthorization();


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