namespace UrlShortener.Api;

public class ApiError
{
    public ApiError(int statuscode, string? message, IEnumerable<string> errors)
    {
        StatusCode = statuscode;
        Message = message;
        Errors = errors ?? throw new ArgumentNullException(nameof(errors));
    }

    public int StatusCode { get; protected set; }
    public string? Message { get; protected set; }
    public int ErrorCount => Errors.Count();
    public IEnumerable<string> Errors { get; protected set; }

    public static ApiError ToBadRequest(Common.Result.Result result)
    {
        return new ApiError(StatusCodes.Status400BadRequest, result.Message, result.Errors);
    }
}
