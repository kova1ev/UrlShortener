using Microsoft.AspNetCore.Components;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using UrlShortener.Application.Common.Models.Links;
using UrlShortener.ClientWasm.Constants;

namespace UrlShortener.ClientWasm.ApiHttpClient
{
    public class LinkHttpService : ILinkHttpService
    {
        private const string LinkUrlAddress = "api/link";
        private readonly NavigationManager _navigationManager;
        private readonly HttpClient _httpClient;
        private JsonSerializerOptions _options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

        public LinkHttpService(HttpClient httpClient, NavigationManager navigationManager)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException($"{nameof(httpClient)}");
            _httpClient.DefaultRequestHeaders.Add(AuthConstant.API_KEY_HEADER, AuthConstant.API_KEY);
            _navigationManager = navigationManager;
        }

        public async Task<HttpResponseResult<IEnumerable<LinkDto>>> GetLinksAsync()
        {
            HttpResponseMessage httpResponse = await _httpClient.GetAsync(LinkUrlAddress);

            return await HandleApiResponse<IEnumerable<LinkDto>>(httpResponse);
        }

        public async Task<HttpResponseResult<LinkDto>> GetLinkByIdAsync(Guid id)
        {
            string route = string.Concat(LinkUrlAddress, "/", id.ToString());
            HttpResponseMessage httpResponse = await _httpClient.GetAsync(route);

            return await HandleApiResponse<LinkDto>(httpResponse);
        }

        public async Task<HttpResponseResult<LinkResponse>> CreateLinkAsync(CreateLinkModel linkModel)
        {
            HttpResponseMessage httpResponse = await _httpClient.PostAsJsonAsync(LinkUrlAddress, linkModel);

            return await HandleApiResponse<LinkResponse>(httpResponse);
        }

        public async Task<HttpResponseResult> DeleteLinkAsync(Guid id)
        {
            string route = string.Concat(LinkUrlAddress, "/", id.ToString());
            HttpResponseMessage httpResponse = await _httpClient.DeleteAsync(route);

            string jsonString = await httpResponse.Content.ReadAsStringAsync();
            if (httpResponse.IsSuccessStatusCode)
            {
                return new HttpResponseResult
                {
                    Status = ResultStatus.Success,
                    StatusCode = httpResponse.StatusCode,
                };
            }
            else
            {
                ApiErrors apiErrors = JsonSerializer.Deserialize<ApiErrors>(jsonString, _options)!;
                return new HttpResponseResult
                {
                    Status = ResultStatus.Errors,
                    StatusCode = httpResponse.StatusCode,
                    ApiErrors = apiErrors
                };
            }
        }



        public async Task<HttpResponseResult<T>> HandleApiResponse<T>(HttpResponseMessage httpResponse)
        {
            string jsonString = await httpResponse.Content.ReadAsStringAsync();
            if (httpResponse.IsSuccessStatusCode)
            {
                if (httpResponse.StatusCode == HttpStatusCode.NoContent)
                {
                    return new HttpResponseResult<T>
                    {
                        Status = ResultStatus.EmptyResult,
                        StatusCode = httpResponse.StatusCode
                    };
                }
                else
                {
                    T value = JsonSerializer.Deserialize<T>(jsonString, _options)!;
                    return new HttpResponseResult<T>
                    {
                        Status = ResultStatus.Success,
                        StatusCode = httpResponse.StatusCode,
                        Value = value
                    };
                }
            }
            else
            {
                ApiErrors apiErrors = JsonSerializer.Deserialize<ApiErrors>(jsonString, _options)!;
                return new HttpResponseResult<T>
                {
                    Status = ResultStatus.Errors,
                    StatusCode = httpResponse.StatusCode,
                    ApiErrors = apiErrors
                };
            }
        }
    }
}
