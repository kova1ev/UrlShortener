using MediatR;
using UrlShortener.Common.Result;

namespace UrlShortener.Application.Links.Commands.UpdateLink;

public class UpdateLinkCommand : IRequest<Result>
{
    public Guid Id { get; set; }
    public string? Alias { get; set; }
    public string? UrlAddress { get; set; }

    public UpdateLinkCommand(UpdateLinkDto updateLinkDto)
    {
        Alias = updateLinkDto.Alias;
        UrlAddress = updateLinkDto.UrlAddress;
        Id = updateLinkDto.Id;
    }
}
