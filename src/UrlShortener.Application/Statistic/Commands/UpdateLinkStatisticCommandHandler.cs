using MediatR;
using Microsoft.EntityFrameworkCore;
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
        LinkStatistic? linkStatistic = await _appDbContext.LinkStatistics.FirstOrDefaultAsync(s => s.Id == request.Id);

        if (linkStatistic == null)
            throw new NullReferenceException("LinkStatistic is null");

        linkStatistic.Clicks++;
        linkStatistic.LastUse = DateTime.UtcNow;

        await _appDbContext.SaveChangesAsync();
        return Unit.Value;
    }
}
