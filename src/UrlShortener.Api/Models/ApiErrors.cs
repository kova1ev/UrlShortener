using System.Collections;
using UrlShortener.Application.Common.Constants;
using UrlShortener.Application.Common.Result;

namespace UrlShortener.Api.Models;

public class ApiErrors
{
    public ApiErrors(int statusCode, string message, IEnumerable<string>? errors = null)
    {
        StatusCode = statusCode;
        Message = message;
        Errors = errors;
    }

    public int StatusCode { get; protected set; }
    public string Message { get; protected set; }
    public IEnumerable<string>? Errors { get; protected set; }

    public static ApiErrors ToBadRequest(Result result)
    {
        return new ApiErrors(StatusCodes.Status400BadRequest, StatusCodeErrorMessage.BadRequestErrorMessage, result.Errors);
    }
}