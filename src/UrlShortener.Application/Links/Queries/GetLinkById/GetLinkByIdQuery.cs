using MediatR;
using UrlShortener.Application.Common.Result;
using UrlShortener.Application.Responses;

namespace UrlShortener.Application.Links.Queries.GetLinkById;

public class GetLinkByIdQuery : IRequest<Result<LinkDetailsResponse>>
{
    public Guid Id { get; set; }

    public GetLinkByIdQuery(Guid id)
    {
        Id = id;
    }
}