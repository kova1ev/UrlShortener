namespace UrlShortener.Api;

public class ApiErrors
{
    public ApiErrors(int statusCode, string message, IEnumerable<string>? errors)
    {
        StatusCode = statusCode;
        Message = message;
        Errors = errors;
    }

    public int StatusCode { get; protected set; }
    public string? Message { get; protected set; }
    public int? ErrorCount => Errors == null ? null : Errors.ToList().Count;
    public IEnumerable<string>? Errors { get; protected set; }

    public static ApiErrors ToBadRequest(Application.Common.Result.Result result)
    {
        return new ApiErrors(StatusCodes.Status400BadRequest, result.Message, result.Errors);
    }
}
