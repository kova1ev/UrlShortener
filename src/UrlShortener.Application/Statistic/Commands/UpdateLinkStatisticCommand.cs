using MediatR;
using UrlShortener.Application.Common.Models;

namespace UrlShortener.Application.Statistic.Commands;

public class UpdateLinkStatisticCommand : IRequest
{
    public Guid Id { get; }
    public UserAgentInfo AgentInfo { get; set; }
    public UpdateLinkStatisticCommand(Guid id, UserAgentInfo agentInfo)
    {
        Id = id;
        AgentInfo = agentInfo;
    }
}
