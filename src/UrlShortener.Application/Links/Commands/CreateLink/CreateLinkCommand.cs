using MediatR;
using UrlShortener.Application.Common.Models.Links;
using UrlShortener.Application.Common.Result;

namespace UrlShortener.Application.Links.Commands.CreateLink;

public class CreateLinkCommand : IRequest<Result<LinkResponse>>
{
    public string UrlAddress { get; }
    public string? Alias { get; }

    public CreateLinkCommand(CreateLinkModel createLinkDto)
    {
        UrlAddress = createLinkDto.UrlAddress;
        Alias = createLinkDto.Alias;
    }
}
