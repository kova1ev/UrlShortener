namespace UrlShortener.Application.Common.Exceptions;

[Serializable]
public class ValidationException : Exception
{
    private const string? DefaultMessage = "Validation errors occurred.";
    public IEnumerable<string> Errors { get; private set; }

    public ValidationException() : base(DefaultMessage)
    {
        Errors = Enumerable.Empty<string>();
    }

    public ValidationException(IEnumerable<string> errors) : this()
    {
        Errors = errors;
    }

    public ValidationException(string message, IEnumerable<string> errors) : base(message)
    {
        Errors = errors;
    }
}