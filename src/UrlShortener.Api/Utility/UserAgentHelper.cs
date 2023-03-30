using DeviceDetectorNET;
using UrlShortener.Application.Common.Models;

namespace UrlShortener.Api.Utility;

public class UserAgentHelper
{
    public UserAgentInfo Parse(string userAgent)
    {
        UserAgentInfo userAgentInfo = new();
        DeviceDetector deviceDetector = new DeviceDetector(userAgent);

        deviceDetector.Parse();
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
