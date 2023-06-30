using MediatR;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Application.Common.Result;
using UrlShortener.Application.Interfaces;

namespace UrlShortener.Application.GlobalStatistics.Queries.GetLinksCountByTime;

public class GetLinksCountByTimeHandler : IRequestHandler<GetLinksCountByTime, Result<int>>
{
    private readonly IAppDbContext _appDbContext;

    public GetLinksCountByTimeHandler(IAppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<Result<int>> Handle(GetLinksCountByTime request, CancellationToken cancellationToken)
    {
        var count = 0;
        var query = _appDbContext.Links.AsNoTracking();

        if (request.StartDate == null && request.EndDate == null)
        {
            var lastWeek = -7;
            count = await query.Where(link => link.DateTimeCreated >= DateTime.UtcNow.AddDays(lastWeek))
                .CountAsync(cancellationToken: cancellationToken);
            return Result<int>.Success(count);
        }

        if (request.StartDate != null)
            query = query.Where(link =>
                link.DateTimeCreated >= request.StartDate.Value.ToDateTime(new TimeOnly(), DateTimeKind.Utc));
        if (request.EndDate != null)
            query = query.Where(link =>
                link.DateTimeCreated <= request.EndDate.Value.ToDateTime(new TimeOnly(), DateTimeKind.Utc));

        count = await query.CountAsync(cancellationToken);

        return Result<int>.Success(count);
    }
}