namespace UrlShortener.Application.Common.Constants;

public static class LinkValidationErrorMessage
{
    public const string ALIAS_TAKEN = "Alias is taken.";
    public const string ALIAS_HAVE_WHITESPACE = "Alias must not contain spaces.";
    public const string LINK_NOT_EXISTING = "Link is not existing.";
    public const string URL_ADDRESS_REQUIRED = "UrlAddress is required.";
    public const string URL_ADDRESS_IS_NOT_URL = "UrlAddress is not url.";
    public const string ID_REQUIRED = "Id is required.";
    public const string ALIAS_BAD_RANGE = "Alias length must be in range 3 - 30 characters.";
}
