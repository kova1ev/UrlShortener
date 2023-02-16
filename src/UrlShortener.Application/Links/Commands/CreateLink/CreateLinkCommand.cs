using MediatR;
using UrlShortener.Application.Dto.Link;

namespace UrlShortener.Application.Links.Commands.CreateLink;

public class CreateLinkCommand : IRequest<Guid>
{
    //TODO оставить дто или сделать проперти?
    public CreateLinkCommand(CreateLinkDto creteLinkDto)
    {
        CreteLinkDto = creteLinkDto;
    }
    public CreateLinkDto CreteLinkDto { get; set; }
}
