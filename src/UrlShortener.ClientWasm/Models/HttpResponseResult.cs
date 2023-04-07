using System.Net;

namespace UrlShortener.ClientWasm.Models;

public enum ResultStatus
{
    Success,
    EmptyResult,
    Errors
}

public class HttpResponseResult
{
    public ResultStatus Status { get; init; }
    public HttpStatusCode StatusCode { get; init; }
    public ApiErrors? ApiErrors { get; init; }

}
public sealed class HttpResponseResult<TResult> : HttpResponseResult
{
    public TResult? Value { get; init; }
}
