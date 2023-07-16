using DeviceDetectorNET;
using UrlShortener.Application.Common.Domain;

namespace UrlShortener.Api.Utility;

public static class UserAgentHelper
{
    private const string UserAgent = "user-agent";
    public static UserAgentInfo TryGetUserAgentInfo(this HttpRequest httpRequest)
    {
        string? userAgent = httpRequest.Headers[UserAgent];
        var deviceDetector = new DeviceDetector(userAgent);
        deviceDetector.Parse();

        UserAgentInfo userAgentInfo = new();
        if (!deviceDetector.IsBot())
        {
            userAgentInfo.Browser = deviceDetector.GetClient().Match.Name;
            userAgentInfo.Os = deviceDetector.GetOs().Match?.Name;
            userAgentInfo.Device = deviceDetector.GetDeviceName();
            userAgentInfo.Brand = deviceDetector.GetBrandName();
            userAgentInfo.Model = deviceDetector.GetModel();
        }

        return userAgentInfo;
    }
}