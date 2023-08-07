using MediatR;
using UrlShortener.Application.Common.Result;
using UrlShortener.Application.Responses;

namespace UrlShortener.Application.Links.Queries.GetMostRedirectedLinks;

public class GetMostRedirectedLinksQuery : IRequest<Result<IEnumerable<LinkCompactResponse>>>
{
    public GetMostRedirectedLinksQuery()
    {
    }
}