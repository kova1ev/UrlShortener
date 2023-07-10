using MediatR;
using UrlShortener.Application.Common.Domain.Links;
using UrlShortener.Application.Common.Result;

namespace UrlShortener.Application.Links.Queries.GetMostRedirectedLinks;

public class GetMostRedirectedLinksQuery : IRequest<Result<IEnumerable<LinkCompactResponse>>>
{
    public GetMostRedirectedLinksQuery()
    {
    }
}