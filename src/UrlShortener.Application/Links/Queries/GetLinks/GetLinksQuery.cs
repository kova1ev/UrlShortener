using MediatR;
using UrlShortener.Application.Common.Models.Links;

namespace UrlShortener.Application.Links.Queries.GetLinks;
public class GetLinksQuery : IRequest<IEnumerable<LinkDto>>
{
}
