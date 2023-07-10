using MediatR;
using Microsoft.Extensions.DependencyInjection;
using UrlShortener.Application.Common.Domain;
using UrlShortener.Application.Common.Result;

namespace UrlShortener.Application.LinkStatistics.Commands;

public class UpdateLinkStatisticCommand : IRequest<Result>
{
    public IServiceScopeFactory ServiceScopeFactory { get; }
    public Guid Id { get; }
    public UserAgentInfo AgentInfo { get; set; }
    public Geolocation Geolocation { get; set; }

    public UpdateLinkStatisticCommand(Guid id, UserAgentInfo agentInfo, Geolocation geolocation,
        IServiceScopeFactory serviceScopeFactory)
    {
        Id = id;
        AgentInfo = agentInfo;
        Geolocation = geolocation;
        ServiceScopeFactory = serviceScopeFactory;
    }
}