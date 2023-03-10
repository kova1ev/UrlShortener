using MediatR;

namespace UrlShortener.Application.Links.Queries.GetLinkByShortName;


public class GetLinkByIdQuery : IRequest<LinkDto>
{
    public Guid Id { get; set; }

    public GetLinkByIdQuery(Guid id)
    {
        Id = id;
    }
}