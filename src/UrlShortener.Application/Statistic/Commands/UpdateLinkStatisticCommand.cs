using MediatR;

namespace UrlShortener.Application.Statistic.Commands;

public class UpdateLinkStatisticCommand : IRequest
{
    public Guid Id { get; }
    public UpdateLinkStatisticCommand(Guid id)
    {
        Id = id;
    }
}
