using UrlShortener.Application.Common.Constants;

namespace UrlShortener.Api.Models;

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

    public static ApiErrors ToBadRequest(Application.Common.Result.Result result)
    {
        return new ApiErrors(StatusCodes.Status400BadRequest, StatusCodeMessage.BAD_REQUEST_MESSAGE, result.Errors);
    }
}
