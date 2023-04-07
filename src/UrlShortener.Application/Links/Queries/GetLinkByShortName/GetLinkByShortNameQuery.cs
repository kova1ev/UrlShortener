using MediatR;
using UrlShortener.Application.Common.Models.Links;
using UrlShortener.Application.Common.Result;

namespace UrlShortener.Application.Links.Queries.GetLinkByShortName;

public class GetLinkByShortNameQuery : IRequest<Result<LinkDetailsResponse>>
{
    public string Alias { get; set; }

    public GetLinkByShortNameQuery(string alias)
    {
        Alias = alias;
    }
}
