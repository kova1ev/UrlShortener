namespace UrlShortener.Application.Common.Exceptions;

public class ObjectNotFoundException : Exception
{
    private const string DefaultMessage = "Object not found.";

    public ObjectNotFoundException(string message) : base(message)
    {
    }

    public ObjectNotFoundException() : base(DefaultMessage)
    {
    }
}