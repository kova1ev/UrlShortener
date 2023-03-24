using MediatR;
using UrlShortener.Application.Common.Models.Links;
using UrlShortener.Application.Common.Result;

namespace UrlShortener.Application.Links.Commands.UpdateLink;

public class UpdateLinkCommand : IRequest<Result>
{
    public Guid Id { get; set; }
    public string? Alias { get; set; }
    public string? UrlAddress { get; set; }

    public UpdateLinkCommand(Guid id, UpdateLinkModel updateLinkDto)
    {
        Id = id;
        UrlAddress = updateLinkDto.UrlAddress;
        Alias = updateLinkDto.Alias;
    }
}
