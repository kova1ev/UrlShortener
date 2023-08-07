using MediatR;
using UrlShortener.Application.Common.Result;
using UrlShortener.Application.Responses;

namespace UrlShortener.Application.Links.Queries.GetLinkByShortName;

public class GetLinkByShortNameQuery : IRequest<Result<LinkDetailsResponse>>
{
    public string Alias { get; set; }

    public GetLinkByShortNameQuery(string alias)
    {
        Alias = alias;
    }
}
