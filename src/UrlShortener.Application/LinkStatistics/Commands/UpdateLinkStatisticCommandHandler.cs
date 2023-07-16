using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using UrlShortener.Application.Common.Domain;
using UrlShortener.Application.Common.Exceptions;
using UrlShortener.Application.Interfaces;

namespace UrlShortener.Application.LinkStatistics.Commands;

public class UpdateLinkStatisticCommandHandler : IRequestHandler<UpdateLinkStatisticCommand>
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private static readonly object _locker = new();
    private readonly ISystemDateTime _systemDateTime;
    private readonly IGeolocationService _geolocationService;
    private readonly ILogger<UpdateLinkStatisticCommandHandler> _logger;

    public UpdateLinkStatisticCommandHandler(ISystemDateTime systemDateTime, IGeolocationService geolocationService,
        ILogger<UpdateLinkStatisticCommandHandler> logger, IServiceScopeFactory serviceScopeFactory)
    {
        _systemDateTime = systemDateTime;
        _geolocationService = geolocationService;
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public Task Handle(UpdateLinkStatisticCommand request, CancellationToken cancellationToken)
    {
        // todo mb try make task runner Queue 
        // very hard test this  
        
        //  Update statistic as background task
        Geolocation geolocation = new();
        if (string.IsNullOrWhiteSpace(request.ClientIp) == false)
        {
            _geolocationService.GetGeolocationDataAsync(request.ClientIp, cancellationToken)
                .ContinueWith(task => Update(request, task.Result), cancellationToken);
        }
        else
        {
            Update(request, geolocation);
        }

        return Task.CompletedTask;
    }

    private void Update(UpdateLinkStatisticCommand request, Geolocation geolocation)
    {
        var lockToken = false;
        try
        {
            Monitor.Enter(_locker, ref lockToken);
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var appDbContext = scope.ServiceProvider.GetRequiredService<IAppDbContext>();
                var linkStatistic = appDbContext.LinkStatistics
                    .Include(st => st.Geolocation)
                    .FirstOrDefault(s => s.Id == request.Id);

                if (linkStatistic == null)
                    throw new ObjectNotFoundException($"LinkStatistic not found with id {request.Id}");

                linkStatistic.Browser = request.AgentInfo.Browser;
                linkStatistic.Os = request.AgentInfo.Os;
                linkStatistic.Clicks++;
                linkStatistic.LastUse = _systemDateTime.UtcNow;

                linkStatistic.Geolocation!.Country = geolocation.Country;
                linkStatistic.Geolocation.Region = geolocation.Region;
                linkStatistic.Geolocation.City = geolocation.City;

                appDbContext.LinkStatistics.Update(linkStatistic);
                appDbContext.SaveChanges();
            }
        }
        catch (ObjectNotFoundException exception)
        {
            _logger.LogError(exception, "Exception occurred while update link statistic {@Message}", exception.Message);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Exception occurred while update link statistic {@Message}", exception.Message);
        }
        finally
        {
            Monitor.Exit(_locker);
        }
    }
}