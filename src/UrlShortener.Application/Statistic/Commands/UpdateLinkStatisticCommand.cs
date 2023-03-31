using MediatR;
using UrlShortener.Application.Common.Models;

namespace UrlShortener.Application.Statistic.Commands;

public class UpdateLinkStatisticCommand : IRequest
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
