using System.Runtime.Serialization;

namespace UrlShortener.Application.Exceptions;

[Serializable]
public class ObjectExistingException : Exception
{
    public ObjectExistingException()
    {
    }

    public ObjectExistingException(string? message) : base(message)
    {
    }

    public ObjectExistingException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected ObjectExistingException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
