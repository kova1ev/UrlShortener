using MediatR;
using UrlShortener.Application.Common.Models.Links;
using UrlShortener.Application.Common.Result;

namespace UrlShortener.Application.Links.Queries.GetLinks;
public class GetLinksQuery : IRequest<Result<IEnumerable<LinkDetailsResponse>>>
{
}
