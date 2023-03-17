
namespace UrlShortener.ClientWasm.ApiHttpClient;

public class ApiErrors
{
    public ApiErrors(int statusCode, string message, IEnumerable<string>? errors)
    {
        StatusCode = statusCode;
        Message = message;
        Errors = errors;
    }

    public int StatusCode { get; protected set; }
    public string Message { get; protected set; }
    public IEnumerable<string>? Errors { get; protected set; }

}
