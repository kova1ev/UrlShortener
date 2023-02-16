using MediatR;
using UrlShortener.Domain.Entity;

namespace UrlShortener.Application.Links.Queries.GetLinkByShortName;


public class GetLinkByIdQuery : IRequest<Link>
{
    public Guid Id { get; set; }

    public GetLinkByIdQuery(Guid id)
    {
        Id = id;
    }
}