namespace UrlShortener.Application.Common.Constants;

public static class StatusCodeErrorMessage
{
    public const string InternalServerErrorMessage = "Internal server error.";
    public const string BadRequestErrorMessage = "One or more errors occurred.";
    public const string ApiKeyErrorMessage = "ApiKey is required.";
    public const string JwtTokenErrorMessage = "JwtToken is required.";
}