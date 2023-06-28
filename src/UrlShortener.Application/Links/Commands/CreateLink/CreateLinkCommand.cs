using MediatR;
using UrlShortener.Application.Common.Domain.Links;
using UrlShortener.Application.Common.Result;
using UrlShortener.Application.Common.Utility;

namespace UrlShortener.Application.Links.Commands.CreateLink;

public class CreateLinkCommand : IRequest<Result<LinkCreatedResponse>>
{
    public string? UrlAddress { get; }
    public string? Alias { get; }

    public CreateLinkCommand(string urlAddress, string alias)
    {
        UrlAddress = urlAddress.TrimAndSetNull();
        Alias = alias.TrimAndSetNull();
    }

}
