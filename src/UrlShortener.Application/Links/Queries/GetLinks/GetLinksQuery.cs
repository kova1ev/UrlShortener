using MediatR;

namespace UrlShortener.Application.Links.Queries.GetLinks;
public class GetLinksQuery : IRequest<IEnumerable<LinkDto>>
{
}
