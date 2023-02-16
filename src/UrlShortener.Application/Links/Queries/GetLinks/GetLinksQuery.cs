using MediatR;
using UrlShortener.Domain.Entity;

namespace UrlShortener.Application.Links.Queries.GetLinks;
public class GetLinksQuery : IRequest<IEnumerable<Link>>
{
}
