using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using System.Text.Json;
using UrlShortener.ClientWasm.Constant;

namespace UrlShortener.ClientWasm.ApiHttpClient
{
    public class LinkHttpClient : ILinkHttpClient
    {
        private readonly NavigationManager _navigationManager;
        private readonly HttpClient _httpClient;
        private JsonSerializerOptions _options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

        public LinkHttpClient(HttpClient httpClient, NavigationManager navigationManager)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException($"{nameof(httpClient)}");
            _httpClient.DefaultRequestHeaders.Add(AuthConstant.API_KEY_HEADER, AuthConstant.API_KEY);
            _navigationManager = navigationManager;
        }

        public async Task<HttpResponseResult<T>> GetAsync<T>(string url)
        {
            HttpResponseMessage httpResponse = await _httpClient.GetAsync(url);

            if (httpResponse.IsSuccessStatusCode)
            {
                // TODO : NoContent Success or Fail result ?!
                if (httpResponse.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return HttpResponseResult<T>.NoContentResult(httpResponse.StatusCode);
                }
                string jsonString = await httpResponse.Content.ReadAsStringAsync();
                T value = JsonSerializer.Deserialize<T>(jsonString, _options)!;
                return HttpResponseResult<T>.SuccessResult(value, httpResponse.StatusCode);
            }

            string errorJsonString = await httpResponse.Content.ReadAsStringAsync();
            ApiErrors apiErrors = JsonSerializer.Deserialize<ApiErrors>(errorJsonString, _options)!;

            return HttpResponseResult<T>.FailureResult(apiErrors, httpResponse.StatusCode);
        }


        public async Task<HttpResponseResult<T>> PostAsync<T>(string url, object data)
        {
            HttpResponseMessage httpResponse = await _httpClient.PostAsJsonAsync("api/link", data);
            if (httpResponse.IsSuccessStatusCode)
            {
                if (typeof(T) == typeof(Unit))
                {
                    return HttpResponseResult<T>.SuccessResult(default!, httpResponse.StatusCode);
                }
                string jsonString = await httpResponse.Content.ReadAsStringAsync();
                T value = JsonSerializer.Deserialize<T>(jsonString, _options)!;
                return HttpResponseResult<T>.SuccessResult(value, httpResponse.StatusCode);
            }

            string errorJsonString = await httpResponse.Content.ReadAsStringAsync();
            ApiErrors apiErrors = JsonSerializer.Deserialize<ApiErrors>(errorJsonString, _options)!;

            return HttpResponseResult<T>.FailureResult(apiErrors, httpResponse.StatusCode);
        }


        public async Task<HttpResponseResult<T>> DeleteAsync<T>(string url)
        {
            HttpResponseMessage httpResponse = await _httpClient.DeleteAsync(url);
            if (httpResponse.IsSuccessStatusCode)
            {
                if (typeof(T) == typeof(Unit))
                {
                    return HttpResponseResult<T>.SuccessResult(default!, httpResponse.StatusCode);
                }
                string jsonString = await httpResponse.Content.ReadAsStringAsync();
                T value = JsonSerializer.Deserialize<T>(jsonString, _options)!;
                return HttpResponseResult<T>.SuccessResult(value, httpResponse.StatusCode);
            }

            string errorJsonString = await httpResponse.Content.ReadAsStringAsync();
            ApiErrors apiErrors = JsonSerializer.Deserialize<ApiErrors>(errorJsonString, _options)!;

            return HttpResponseResult<T>.FailureResult(apiErrors, httpResponse.StatusCode);
        }
    }
}
