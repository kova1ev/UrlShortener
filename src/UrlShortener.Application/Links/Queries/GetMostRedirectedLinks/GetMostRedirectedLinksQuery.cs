using MediatR;
using UrlShortener.Application.Common.Domain.Links;
using UrlShortener.Application.Common.Result;
using UrlShortener.Entity;

namespace UrlShortener.Application.Links.Queries.GetMostRedirectedLinks;

public class GetMostRedirectedLinksQuery : IRequest<Result<IEnumerable<LinkCompactResponse>>>
{
    public GetMostRedirectedLinksQuery()
    {
    }
}