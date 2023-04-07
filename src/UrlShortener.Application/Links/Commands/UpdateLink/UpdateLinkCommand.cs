using MediatR;
using UrlShortener.Application.Common.Result;
using UrlShortener.Application.Common.Utility;

namespace UrlShortener.Application.Links.Commands.UpdateLink;

public class UpdateLinkCommand : IRequest<Result>
{
    public Guid Id { get; set; }
    public string? Alias { get; set; }
    public string? UrlAddress { get; set; }

    public UpdateLinkCommand(Guid id, string urlAddress, string alias)
    {
        Id = id;
        UrlAddress = urlAddress.TrimAndSetNull(); ;
        Alias = alias.TrimAndSetNull(); ;
    }
}
