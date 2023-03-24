namespace UrlShortener.Application.Common.Utility;

public static class StringExtension
{
    public static string? TrimAndSetNull(this string? str)
    {
        str = str?.Trim();
        str = str?.Length == 0 ? null : str;
        return str;
    }
}
