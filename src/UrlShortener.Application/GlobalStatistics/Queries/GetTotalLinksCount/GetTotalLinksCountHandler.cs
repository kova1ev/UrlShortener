using MediatR;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Application.Common.Result;
using UrlShortener.Application.Interfaces;

namespace UrlShortener.Application.GlobalStatistics.Queries.GetTotalLinksCount;

public class GetTotalLinksCountHandler : IRequestHandler<GetTotalLinksCountQuery, Result<int>>
{
    private readonly IAppDbContext _appDbContext;

    public GetTotalLinksCountHandler(IAppDbContext appDbContext)
    {
        _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
    }

    public async Task<Result<int>> Handle(GetTotalLinksCountQuery request, CancellationToken cancellationToken)
    {
        var linkCount = await _appDbContext.Links.AsNoTracking().CountAsync(cancellationToken);

        return Result<int>.Success(linkCount);
    }
}