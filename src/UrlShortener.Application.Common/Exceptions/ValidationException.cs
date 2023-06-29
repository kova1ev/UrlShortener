using System.Runtime.Serialization;

namespace UrlShortener.Application.Common.Exceptions;

[Serializable]
public class ValidationException : Exception
{
    public IEnumerable<string> Errors { get; private set; }

    public ValidationException() : base("Validation errors occurred.")
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

