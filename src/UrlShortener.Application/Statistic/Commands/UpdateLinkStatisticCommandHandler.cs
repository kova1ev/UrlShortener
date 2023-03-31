using MediatR;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Application.Common.Constants;
using UrlShortener.Application.Common.Exceptions;
using UrlShortener.Data;
using UrlShortener.Domain.Entity;

namespace UrlShortener.Application.Statistic.Commands;

public class UpdateLinkStatisticCommandHandler : IRequestHandler<UpdateLinkStatisticCommand, Unit>
{
    private readonly AppDbContext _appDbContext;

    public UpdateLinkStatisticCommandHandler(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<Unit> Handle(UpdateLinkStatisticCommand request, CancellationToken cancellationToken)
    {
        LinkStatistic? linkStatistic = await _appDbContext.LinkStatistics
            .Include(st => st.Geolocation)
            .FirstOrDefaultAsync(s => s.Id == request.Id);

        if (linkStatistic == null)
            throw new ObjectNotFoundException(LinkStatisticsErrorMessage.NOT_FOUND);

        linkStatistic.Browser = request.AgentInfo.Browser;
        linkStatistic.Os = request.AgentInfo.Os;
        linkStatistic.Clicks++;
        linkStatistic.LastUse = DateTime.UtcNow;

        linkStatistic.Geolocation.Country = request.Geolocation.Country;
        linkStatistic.Geolocation.Region = request.Geolocation.Region;
        linkStatistic.Geolocation.City = request.Geolocation.City;


        await _appDbContext.SaveChangesAsync();
        return Unit.Value;
    }
}
