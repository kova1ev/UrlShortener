using MediatR;
using UrlShortener.Application.Common.Links;
using UrlShortener.Application.Common.Result;

namespace UrlShortener.Application.Links.Commands.CreateLink;

public class CreateLinkCommand : IRequest<Result<LinkResponse>>
{
    public string UrlAddress { get; }
    public string? Alias { get; }

    public CreateLinkCommand(CreateLinkDto createLinkDto)
    {
        UrlAddress = createLinkDto.UrlAddress.Trim()!;
        Alias = createLinkDto.Alias?.Trim();
    }

}
