using UrlShortener.Application.Common.Models.Links;

namespace UrlShortener.ClientWasm.ApiHttpClient;

public interface ILinkHttpService
{
    Task<HttpResponseResult<IEnumerable<LinkDto>>> GetLinksAsync();
    Task<HttpResponseResult<LinkDto>> GetLinkByIdAsync(Guid id);
    Task<HttpResponseResult<LinkResponse>> CreateLinkAsync(CreateLinkModel linkModel);
    Task<HttpResponseResult> DeleteLinkAsync(Guid id);
    // TODO : Update 
}
