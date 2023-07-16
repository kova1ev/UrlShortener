namespace UrlShortener.Application.Common.Constants;

public static class LinkValidationErrorMessage
{
    public const string AliasTaken = "Alias is taken.";
    public const string AliasHaveWhitespace = "Alias must not contain spaces.";
    public const string LinkNotExisting = "Link is not existing.";
    public const string UrlAddressRequired = "UrlAddress is required.";
    public const string UrlAddressIsNotUrl = "UrlAddress is not url.";
    public const string IdRequired = "Id is required.";
    public const string AliasBadRange = "Alias length must be in range 3 - 30 characters.";
}
