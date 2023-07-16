namespace UrlShortener.Api.Utility;

public static class ClientIpHelper
{
    // todo try get from config appOptions  
    private const string CfConnectingIp = "CF-Connecting-IP";
    private const string XForwardedFor = "X-Forwarded-For";

    public static string? GetClientIp(this HttpRequest httpRequest)
    {
        string? clientIp = httpRequest.Headers[CfConnectingIp];
        if (clientIp != null)
        {
            return clientIp;
        }

        string? xForwardedFor = httpRequest.Headers[XForwardedFor];
        clientIp = Parse(xForwardedFor);

        return clientIp;
    }


    private static string? Parse(string? xForwardedFor)
    {
        return xForwardedFor?.Split(',', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault()?.Trim();
    }
}