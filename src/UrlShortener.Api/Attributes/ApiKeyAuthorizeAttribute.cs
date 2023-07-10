using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using UrlShortener.Api.Models;
using UrlShortener.Api.Utility;

namespace UrlShortener.Api.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class ApiKeyAuthorizeAttribute : Attribute, IAuthorizationFilter
{
    public string Message { get; set; } = "ApiKey is required";

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (context.HttpContext.Request.Headers.TryGetValue(ApiKeyOptions.ApiKeyHeaderName,
                out var apiKeyFromHeaders) == false)
        {
            context.Result = new UnauthorizedObjectResult(
                new ApiErrors(StatusCodes.Status401Unauthorized, Message, null));
            return;
        }

        var configuration = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
        var apiKeyFromSettings = configuration.GetValue<string>(ApiKeyOptions.ApiKeySectionValue)
                                 ?? throw new ArgumentNullException(nameof(ApiKeyOptions.ApiKeySectionValue));

        if (apiKeyFromHeaders.Equals(apiKeyFromSettings) == false)
        {
            context.Result = new UnauthorizedObjectResult(
                new ApiErrors(StatusCodes.Status401Unauthorized, Message, null));
        }
    }
}