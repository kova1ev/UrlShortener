using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using UrlShortener.ClientWasm.Constants;
using UrlShortener.ClientWasm.Models;

namespace UrlShortener.ClientWasm.Services;

public interface ILinkHttpService
{
    Task<HttpResponseResult<IEnumerable<LinkDetailsViewModel>>> GetLinksAsync();
    Task<HttpResponseResult<LinkDetailsViewModel>> GetLinkByIdAsync(Guid id);
    Task<HttpResponseResult<LinkCreatedViewModel>> CreateLinkAsync(CreateLinkModel linkModel);
    Task<HttpResponseResult> DeleteLinkAsync(Guid id);
    // TODO : Update 
}

public class LinkHttpService : ILinkHttpService
{
    private const string ROUTE = "api/link";
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;
    private readonly NavigationManager _navigationManager;
    private readonly AuthenticationStateProvider _authenticationStateProvider;

    private readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };


    public LinkHttpService(HttpClient httpClient, ILocalStorageService localStorage, NavigationManager navigationManager, AuthenticationStateProvider authenticationStateProvider)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException($"{nameof(httpClient)}");
        _httpClient.DefaultRequestHeaders.Add(AuthConstant.API_KEY_HEADER, AuthConstant.API_KEY);
        _localStorage = localStorage;
        _navigationManager = navigationManager;
        _authenticationStateProvider = authenticationStateProvider;
    }

    public async Task<HttpResponseResult<IEnumerable<LinkDetailsViewModel>>> GetLinksAsync()
    {
        await AddAuthorizationHeader();
        HttpResponseMessage httpResponse = await _httpClient.GetAsync(ROUTE);

        return await HandleApiResponse<IEnumerable<LinkDetailsViewModel>>(httpResponse);
    }

    public async Task<HttpResponseResult<LinkDetailsViewModel>> GetLinkByIdAsync(Guid id)
    {
        await AddAuthorizationHeader();
        string route = string.Concat(ROUTE, "/", id.ToString());
        HttpResponseMessage httpResponse = await _httpClient.GetAsync(route);

        return await HandleApiResponse<LinkDetailsViewModel>(httpResponse);
    }

    public async Task<HttpResponseResult<LinkCreatedViewModel>> CreateLinkAsync(CreateLinkModel linkModel)
    {
        HttpResponseMessage httpResponse = await _httpClient.PostAsJsonAsync(ROUTE, linkModel);

        return await HandleApiResponse<LinkCreatedViewModel>(httpResponse);
    }

    public async Task<HttpResponseResult> DeleteLinkAsync(Guid id)
    {
        await AddAuthorizationHeader();
        string route = string.Concat(ROUTE, "/", id.ToString());
        HttpResponseMessage httpResponse = await _httpClient.DeleteAsync(route);

        return await HandleApiResponse<LinkCreatedViewModel>(httpResponse);
    }


    private async Task AddAuthorizationHeader()
    {
        string? token = await _localStorage.GetItemAsync<string>(AuthConstant.TOKEN_KEY);
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", token);
    }

    private async Task<HttpResponseResult<T>> HandleApiResponse<T>(HttpResponseMessage httpResponse)
    {
        string jsonString = await httpResponse.Content.ReadAsStringAsync();

        switch (httpResponse.StatusCode)
        {
            case HttpStatusCode.NoContent:
                return new HttpResponseResult<T>
                {
                    Status = ResultStatus.EmptyResult,
                    StatusCode = httpResponse.StatusCode
                };
            case HttpStatusCode.OK:
                T value = JsonSerializer.Deserialize<T>(jsonString, _options)!;
                return new HttpResponseResult<T>
                {
                    Status = ResultStatus.Success,
                    StatusCode = httpResponse.StatusCode,
                    Value = value
                };
            case HttpStatusCode.Unauthorized:
                ((CustomAuthenticationStateProvider)_authenticationStateProvider).LogOut();
                return new HttpResponseResult<T>
                {
                    Status = ResultStatus.Errors,
                    StatusCode = httpResponse.StatusCode
                };
            case HttpStatusCode.BadRequest or HttpStatusCode.InternalServerError:
                ApiErrors apiErrors = JsonSerializer.Deserialize<ApiErrors>(jsonString, _options)!;
                return new HttpResponseResult<T>
                {
                    Status = ResultStatus.Errors,
                    StatusCode = httpResponse.StatusCode,
                    ApiErrors = apiErrors
                };
            default:
                return new HttpResponseResult<T>
                {
                    Status = ResultStatus.Errors,
                    StatusCode = httpResponse.StatusCode,
                };
        }

        //if (httpResponse.IsSuccessStatusCode)
        //{
        //    if (httpResponse.StatusCode == HttpStatusCode.NoContent)
        //    {
        //        return new HttpResponseResult<T>
        //        {
        //            Status = ResultStatus.EmptyResult,
        //            StatusCode = httpResponse.StatusCode
        //        };
        //    }
        //    else
        //    {
        //        T value = JsonSerializer.Deserialize<T>(jsonString, _options)!;
        //        return new HttpResponseResult<T>
        //        {
        //            Status = ResultStatus.Success,
        //            StatusCode = httpResponse.StatusCode,
        //            Value = value
        //        };
        //    }
        //}
        //else
        //{
        //    if (httpResponse.StatusCode == HttpStatusCode.Unauthorized)
        //    {
        //        ((CustomAuthenticationStateProvider)_authenticationStateProvider).LogOut();
        //        return new HttpResponseResult<T>
        //        {
        //            Status = ResultStatus.Errors,
        //            StatusCode = httpResponse.StatusCode
        //        };
        //    }
        //    else
        //    {

        //        ApiErrors apiErrors = JsonSerializer.Deserialize<ApiErrors>(jsonString, _options)!;
        //        return new HttpResponseResult<T>
        //        {
        //            Status = ResultStatus.Errors,
        //            StatusCode = httpResponse.StatusCode,
        //            ApiErrors = apiErrors
        //        };
        //    }
        //}
    }



}
