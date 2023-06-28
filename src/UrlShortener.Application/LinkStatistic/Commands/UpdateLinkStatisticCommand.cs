using MediatR;
using UrlShortener.Application.Common.Domain;
using UrlShortener.Application.Common.Result;

namespace UrlShortener.Application.Statistic.Commands;

public class UpdateLinkStatisticCommand : IRequest<Result>
{
    public Guid Id { get; }
    public UserAgentInfo AgentInfo { get; set; }
    public Geolocation Geolocation { get; set; }
    public UpdateLinkStatisticCommand(Guid id, UserAgentInfo agentInfo, Geolocation geolocation)
    {
        Id = id;
        AgentInfo = agentInfo;
        Geolocation = geolocation;
    }
}
