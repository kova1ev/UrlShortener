using System.Text.Json;
using System.Text.Json.Serialization;
using UrlShortener.Api.Models;
using UrlShortener.Application.Common.Constants;
using UrlShortener.Application.Common.Exceptions;

namespace UrlShortener.Api.Middleware;

public class AppExceptionHandlerMiddleware
{
    private readonly JsonSerializerOptions options = new JsonSerializerOptions()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
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
        ApiErrors apiError;
        string resultJsonString = string.Empty;
        switch (exception)
        {
            case ValidationException validation:
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                apiError = new ApiErrors(StatusCodes.Status400BadRequest,
                   StatusCodeMessage.BAD_REQUEST_MESSAGE,
                   validation.Errors);
                resultJsonString = JsonSerializer.Serialize(apiError, options);
                break;
            default:
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                // todo :  log error BeginScope?
                _logger.LogError(exception, "Exception occurred: {Message}", exception.Message);
                apiError = new ApiErrors(StatusCodes.Status500InternalServerError,
                    StatusCodeMessage.INTERNAL_SERVER_ERROR,
                    new[] { exception.Message });
                resultJsonString = JsonSerializer.Serialize(apiError, options);
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

