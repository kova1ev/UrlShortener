using MediatR;
using UrlShortener.Common.Result;

namespace UrlShortener.Application.Links.Commands.DeleteLink;

public class DeleteLinkCommand : IRequest<Result>
{
    public Guid Id { get; }

    public DeleteLinkCommand(Guid id)
    {
        Id = id;
    }
}
