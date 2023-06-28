namespace UrlShortener.Api.Utility;

public class ClientIpHelper
{
    public string? GetClientIpByCloudFlare(HttpContext httpContext)
    {
        //FOR CLOUDFLARE 
        string? clientIp = httpContext.Request.Headers["CF-Connecting-IP"];
        if (clientIp == null)
            clientIp = httpContext.Request.Headers["True-Client-IP"];
        if (clientIp == null)
        {
            string? x_forwarded_for = httpContext.Request.Headers["X-Forwarded-For"];
            clientIp = x_forwarded_for?.Split(',')?.FirstOrDefault();
        }

        return clientIp;
    }
}