using System.Net;

namespace UrlShortener.ClientWasm.ApiHttpClient;

public sealed class HttpResponseResult<TValue>
{
    private readonly TValue? _value;

    private HttpResponseResult(bool success, HttpStatusCode statusCode, TValue? value, ApiErrors? apiErrors)
    {
        Success = success;
        StatusCode = statusCode;
        _value = value;
        ApiErrors = apiErrors;
    }
    public HttpStatusCode StatusCode { get; }
    public ApiErrors? ApiErrors { get; }
    public bool Success { get; }
    public bool HasValue => Success;
    public TValue? Value
    {
        get
        {
            return Success ? _value : throw new InvalidOperationException("Value is null.");
        }
    }

    public static HttpResponseResult<TValue> SuccessResult(TValue value, HttpStatusCode statusCode)
    {
        if (value == null)
            throw new ArgumentNullException(nameof(value), "Can not be null");
        return new HttpResponseResult<TValue>(true, statusCode, value, null);
    }

    public static HttpResponseResult<TValue> FailureResult(ApiErrors apiErrors, HttpStatusCode statusCode)
    {
        if (apiErrors == null)
            throw new ArgumentNullException(nameof(apiErrors), "Can not be null");
        return new HttpResponseResult<TValue>(false, statusCode, default, apiErrors);
    }

    public static HttpResponseResult<TValue> NoContentResult(HttpStatusCode statusCode)
    {
        return new HttpResponseResult<TValue>(false, statusCode, default, default);
    }
}



/// <summary>
/// Empty object for return void result
/// </summary>
public struct Unit
{

}