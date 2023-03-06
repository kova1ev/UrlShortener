namespace UrlShortener.Api.Models;

public class ResultResponse
{
    public ResultResponse(bool success, string message)
    {
        Success = success;
        Message = message;
    }

    public bool Success { get; set; } = true;
    public string Message { get; set; }
}
