using MediatR;
using UrlShortener.Domain.Entity;

namespace UrlShortener.Application.Links.Queries.GetLinkByShortName;

public class GetLinkByShortNameQuery : IRequest<Link>
{
    public string ShortName { get; set; }

    public GetLinkByShortNameQuery(string shortName)
    {
        ShortName = shortName;
    }
}
