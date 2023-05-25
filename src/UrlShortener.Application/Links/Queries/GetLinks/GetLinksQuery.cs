using MediatR;
using UrlShortener.Application.Common.Models;
using UrlShortener.Application.Common.Models.Links;
using UrlShortener.Application.Common.Result;

namespace UrlShortener.Application.Links.Queries.GetLinks;
public class GetLinksQuery : IRequest<Result<FilteredPagedData<LinkCompactResponse>>>
{

    public LinksRequestParameters RequestParameters { get; }

    public GetLinksQuery(LinksRequestParameters requestParameters)
    {
        RequestParameters = requestParameters;
    }
}
