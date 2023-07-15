using MediatR;
using Microsoft.Extensions.DependencyInjection;
using UrlShortener.Application.Common.Domain;
using UrlShortener.Application.Common.Result;

namespace UrlShortener.Application.LinkStatistics.Commands;

public class UpdateLinkStatisticCommand :IRequest
{

    public Guid Id { get; }
    public UserAgentInfo AgentInfo { get;  }
    public string? ClientIp { get;}

    public UpdateLinkStatisticCommand(Guid id, UserAgentInfo agentInfo, string? clientIp
      )
    {
        Id = id;
        AgentInfo = agentInfo;
        ClientIp = clientIp;
      
    }
}