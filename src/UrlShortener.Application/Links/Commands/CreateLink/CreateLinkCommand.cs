using MediatR;
using UrlShortener.Common.Result;

namespace UrlShortener.Application.Links.Commands.CreateLink;

public class CreateLinkCommand : IRequest<Result<LinkResponse>>
{
    public string UrlAddress { get; }
    public string? Alias { get; }

    public CreateLinkCommand(CreateLinkDto creteLinkDto)
    {
        UrlAddress = creteLinkDto.UrlAddress;
        Alias = creteLinkDto.Alias;
    }

}
