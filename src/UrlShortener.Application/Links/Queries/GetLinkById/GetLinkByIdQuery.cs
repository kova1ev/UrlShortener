using MediatR;
using UrlShortener.Application.Common.Models.Links;
using UrlShortener.Application.Common.Result;

namespace UrlShortener.Application.Links.Queries.GetLinkByShortName;


public class GetLinkByIdQuery : IRequest<Result<LinkDto>>
{
    public Guid Id { get; set; }

    public GetLinkByIdQuery(Guid id)
    {
        Id = id;
    }
}