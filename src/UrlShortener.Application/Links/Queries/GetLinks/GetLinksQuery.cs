using MediatR;
using UrlShortener.Application.Common.Dto;
using UrlShortener.Application.Common.Result;
using UrlShortener.Application.Responses;

namespace UrlShortener.Application.Links.Queries.GetLinks;

public class GetLinksQuery : IRequest<Result<FilteredPagedData<LinkCompactResponse>>>
{
    public LinksRequestParameters RequestParameters { get; }

    public GetLinksQuery(LinksRequestParameters requestParameters)
    {
        RequestParameters = requestParameters;
    }
}
