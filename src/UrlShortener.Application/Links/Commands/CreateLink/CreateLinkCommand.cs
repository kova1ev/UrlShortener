using MediatR;
using UrlShortener.Application.Dto.Link;
using UrlShortener.Application.Responses;

namespace UrlShortener.Application.Links.Commands.CreateLink;

public class CreateLinkCommand : IRequest<CommandResult<Guid>>
{
    //TODO оставить дто или сделать проперти?
    public CreateLinkCommand(CreateLinkDto creteLinkDto)
    {
        CreteLinkDto = creteLinkDto;
    }
    public CreateLinkDto CreteLinkDto { get; set; }
}
