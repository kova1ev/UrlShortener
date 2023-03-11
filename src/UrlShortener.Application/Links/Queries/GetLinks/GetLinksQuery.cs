using MediatR;
using UrlShortener.Application.Common.Links;

namespace UrlShortener.Application.Links.Queries.GetLinks;
public class GetLinksQuery : IRequest<IEnumerable<LinkDto>>
{
}
