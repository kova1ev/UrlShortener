using System.Text;
using System.Text.Json;
using UrlShortener.ClientWasm.Constants;
using UrlShortener.ClientWasm.Models;

namespace UrlShortener.ClientWasm.Services;

public interface IAuthenticationService
{
    Task<HttpResponseResult<JwtToken>> GetToken(LoginModel login);
    Task<bool> ValidCheck(string jwtToken);
}

public class AuthenticationService : IAuthenticationService
{
    private const string TOKEN_ROUTE = "/api/auth/token";
    private const string BASE_ROUTE = "/api/auth";
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };

    public AuthenticationService(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException($"{nameof(httpClient)}");
        _httpClient.DefaultRequestHeaders.Add(AuthConstant.API_KEY_HEADER, AuthConstant.API_KEY);
    }

    public async Task<HttpResponseResult<JwtToken>> GetToken(LoginModel login)
    {
        string jsonContent = JsonSerializer.Serialize(login);
        StringContent stringContent = new(jsonContent, Encoding.UTF8, "application/json");

        HttpResponseMessage httpResponse = await _httpClient.PostAsync(TOKEN_ROUTE, stringContent);
        string json = await httpResponse.Content.ReadAsStringAsync();

        if (httpResponse.IsSuccessStatusCode)
        {
            JwtToken token = JsonSerializer.Deserialize<JwtToken>(json, _options)!;
            return new HttpResponseResult<JwtToken>()
            {
                StatusCode = httpResponse.StatusCode,
                Status = ResultStatus.Success,
                Value = token
            };
        }
        ApiErrors apiErrors = JsonSerializer.Deserialize<ApiErrors>(json, _options);
        return new HttpResponseResult<JwtToken>()
        {
            StatusCode = httpResponse.StatusCode,
            Status = ResultStatus.Errors,
            ApiErrors = apiErrors
        };
    }



    public async Task<bool> ValidCheck(string jwtToken)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", jwtToken);
        HttpResponseMessage httpResponse = await _httpClient.GetAsync(BASE_ROUTE);

        if (httpResponse.IsSuccessStatusCode)
        {
            return true;
        }

        return false;
    }

}

