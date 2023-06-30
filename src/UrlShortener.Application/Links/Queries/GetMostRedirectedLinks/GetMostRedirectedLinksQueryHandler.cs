using MediatR;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Application.Common.Domain.Links;
using UrlShortener.Application.Common.Result;
using UrlShortener.Application.Interfaces;

namespace UrlShortener.Application.Links.Queries.GetMostRedirectedLinks;

public class GetMostRedirectedLinksQueryHandler : IRequestHandler<GetMostRedirectedLinksQuery,
    Result<IEnumerable<LinkCompactResponse>>>
{
    private readonly IAppDbContext _appDbContext;

    public GetMostRedirectedLinksQueryHandler(IAppDbContext appDbContext)
    {
        _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
    }

    public async Task<Result<IEnumerable<LinkCompactResponse>>> Handle(GetMostRedirectedLinksQuery request,
        CancellationToken cancellationToken)
    {
        // Todo: Make get page or make 'topRedirected' from request ??   
        int topRedirected = 10;
        var mostRedirectedLinks = await _appDbContext.Links.AsNoTracking()
            .OrderByDescending(l => l.LinkStatistic!.Clicks)
            .ThenByDescending(l => l.DateTimeCreated)
            .Select(l => LinkCompactResponse.MapFromLink(l))
            .Take(topRedirected)
            .ToArrayAsync(cancellationToken);

        return Result<IEnumerable<LinkCompactResponse>>.Success(mostRedirectedLinks);
    }
}