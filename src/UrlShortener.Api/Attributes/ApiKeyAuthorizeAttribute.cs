using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using UrlShortener.Api.Authentication;
using UrlShortener.Api.Models;

namespace UrlShortener.Api.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class ApiKeyAuthorizeAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (context.HttpContext.Request.Headers.TryGetValue(Security.ApiKeyHeaderName, out StringValues apiKeyFromHeaders) == false)
        {
            context.Result = new UnauthorizedObjectResult(
                new ApiErrors(StatusCodes.Status401Unauthorized, "ApiKey is required", null));
            return;
        }

        IConfiguration configuration = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
        string apiKeyFromSettings = configuration.GetValue<string>(Security.ApiKeySectionValue)
            ?? throw new ArgumentNullException("ApiKey is null", nameof(Security.ApiKeySectionValue));

        if (apiKeyFromHeaders.Equals(apiKeyFromSettings) == false)
        {
            context.Result = new UnauthorizedObjectResult(
                new ApiErrors(StatusCodes.Status401Unauthorized, "ApiKey is required", null));
            return;
        }
    }
}

