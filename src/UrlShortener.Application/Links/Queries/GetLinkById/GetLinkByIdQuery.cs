using MediatR;
using UrlShortener.Application.Common.Domain.Links;
using UrlShortener.Application.Common.Result;

namespace UrlShortener.Application.Links.Queries.GetLinkById;

public class GetLinkByIdQuery : IRequest<Result<LinkDetailsResponse>>
{
    public Guid Id { get; set; }

    public GetLinkByIdQuery(Guid id)
    {
        Id = id;
    }
}