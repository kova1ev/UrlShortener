using MediatR;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Application.Common.Constants;
using UrlShortener.Application.Common.Result;
using UrlShortener.Application.Interfaces;
using UrlShortener.Entity;

namespace UrlShortener.Application.Statistic.Commands;

public class UpdateLinkStatisticCommandHandler : IRequestHandler<UpdateLinkStatisticCommand, Result>
{
    private readonly IAppDbContext _appDbContext;
    private readonly ISystemDateTime _systemDateTime;

    public UpdateLinkStatisticCommandHandler(IAppDbContext appDbContext, ISystemDateTime systemDateTime)
    {
        _appDbContext = appDbContext;
        _systemDateTime = systemDateTime;
    }

    public async Task<Result> Handle(UpdateLinkStatisticCommand request, CancellationToken cancellationToken)
    {
        LinkStatistic? linkStatistic = await _appDbContext.LinkStatistics
            .Include(st => st.Geolocation)
            .FirstOrDefaultAsync(s => s.Id == request.Id);

        if (linkStatistic == null)
            return Result.Failure(new string[] { LinkStatisticsErrorMessage.NotFound });

        linkStatistic.Browser = request.AgentInfo.Browser;
        linkStatistic.Os = request.AgentInfo.Os;
        linkStatistic.Clicks++;
        linkStatistic.LastUse = _systemDateTime.UtcNow;

        linkStatistic.Geolocation.Country = request.Geolocation.Country;
        linkStatistic.Geolocation.Region = request.Geolocation.Region;
        linkStatistic.Geolocation.City = request.Geolocation.City;

        _appDbContext.LinkStatistics.Update(linkStatistic);
        await _appDbContext.SaveChangesAsync();

        return Result.Success();
    }
}
