using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using UrlShortener.Application.Common.Exceptions;

namespace UrlShortener.Api.Middleware;

public class AppExceptionHandlerMiddleware
{
    private readonly JsonSerializerSettings options = new JsonSerializerSettings()
    {
        ContractResolver = new CamelCasePropertyNamesContractResolver(),
        NullValueHandling = NullValueHandling.Ignore
    };

    private readonly RequestDelegate _next;
    private readonly ILogger<AppExceptionHandlerMiddleware> _logger;
    public AppExceptionHandlerMiddleware(RequestDelegate next, ILogger<AppExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next.Invoke(context);
        }
        catch (Exception ex)
        {
            await ExceptionHandler(context, ex);

        }
    }

    public async Task ExceptionHandler(HttpContext context, Exception exception)
    {

        context.Response.ContentType = "application/json";
        string resultJsonString = string.Empty;
        switch (exception)
        {
            case ValidationException validation:
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                ApiErrors apiError = new ApiErrors(StatusCodes.Status400BadRequest, validation.Message, validation.Errors);
                resultJsonString = JsonConvert.SerializeObject(apiError, options);
                break;
            default:
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                //TODO  log error
                _logger.LogCritical("{0} \n {1} \n {2}", exception.Source, exception.StackTrace, exception.Message);
                string message = "Internal Server Error";
                ApiErrors apiErrors = new ApiErrors(StatusCodes.Status500InternalServerError, message, null);
                resultJsonString = JsonConvert.SerializeObject(apiErrors, options);
                break;
        }

        await context.Response.WriteAsync(resultJsonString);
    }

}

public static class AppExceptionHandlerExtension
{
    public static IApplicationBuilder UseAppExceptionMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<AppExceptionHandlerMiddleware>();
    }
}

