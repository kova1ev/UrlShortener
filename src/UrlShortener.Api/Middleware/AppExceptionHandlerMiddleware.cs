using System.Text.Json;
using System.Text.Json.Serialization;
using UrlShortener.Api.Models;
using UrlShortener.Application.Common.Constants;
using UrlShortener.Application.Common.Exceptions;

namespace UrlShortener.Api.Middleware;

public class AppExceptionHandlerMiddleware
{
    private readonly ILogger<AppExceptionHandlerMiddleware> _logger;

    private readonly RequestDelegate _next;

    private readonly JsonSerializerOptions _options = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

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
        var resultJsonString = string.Empty;
        switch (exception)
        {
            case OperationCanceledException:
                context.Response.StatusCode = 409;
                _logger.LogInformation(exception, "Canceled request {@Path}", context.Request.Path);
                break;
            case ValidationException validation:
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                apiError = new ApiErrors(StatusCodes.Status400BadRequest,
                    StatusCodeErrorMessage.BadRequestErrorMessage,
                    validation.Errors);
                resultJsonString = JsonSerializer.Serialize(apiError, _options);
                break;
            default:
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                _logger.LogError(exception, "Exception occurred: {@Message}", exception.Message);
                apiError = new ApiErrors(StatusCodes.Status500InternalServerError,
                    StatusCodeErrorMessage.InternalServerErrorMessage,
                    new[] { exception.Message });
                resultJsonString = JsonSerializer.Serialize(apiError, _options);
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