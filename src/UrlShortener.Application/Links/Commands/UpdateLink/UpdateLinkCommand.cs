using MediatR;
using UrlShortener.Application.Common.Result;

namespace UrlShortener.Application.Links.Commands.UpdateLink;

public class UpdateLinkCommand : IRequest<Result>
{
    public Guid Id { get; }
    public string? Alias { get; }
    public string? UrlAddress { get; }

    public UpdateLinkCommand(Guid id, string? urlAddress, string? alias)
    {
        Id = id;
        UrlAddress = urlAddress;
        Alias = alias;
    }
}