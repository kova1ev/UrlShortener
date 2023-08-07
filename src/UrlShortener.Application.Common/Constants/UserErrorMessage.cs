namespace UrlShortener.Application.Common.Constants;

public static class UserErrorMessage
{
    public const string UserIdIsRequired = "User Id is required.";
    public const string UserNotFound = "User not found.";
    public const string UserNameIsRequired = "User Name is required.";
    public const string UserNameIsBadRange = "User Name length must be in range 3 - 50 characters.";
    public const string UserEmailIsRequired = "User Email is required.";
    public const string InvalidUserEmail = "User Email is invalid.";
    public const string UserPasswordIsRequired = "User Password is required.";
}