namespace UrlShortener.ClientWasm.ApiHttpClient;

public interface ILinkHttpClient
{
    Task<HttpResponseResult<T>> GetAsync<T>(string url);
    Task<HttpResponseResult<T>> PostAsync<T>(string url, object data);
    Task<HttpResponseResult<T>> DeleteAsync<T>(string url);
    // TODO : Update 
}
