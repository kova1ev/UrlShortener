using MediatR;
using UrlShortener.Application.Commands.Links;
using UrlShortener.Application.Common.Result;

namespace UrlShortener.Application.Links.Commands.UpdateLink;

public class UpdateLinkCommand : IRequest<Result>
{
    public Guid Id { get; set; }
    public string? Alias { get; set; }
    public string? UrlAddress { get; set; }

    public UpdateLinkCommand(Guid id, UpdateLinkDto updateLinkDto)
    {
        Id = id;
        UrlAddress = updateLinkDto.UrlAddress?.Trim();
        Alias = updateLinkDto.Alias?.Trim();
    }
}
