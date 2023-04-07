using MediatR;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Application.Common.Models.Links;
using UrlShortener.Application.Interfaces;


namespace UrlShortener.Application.Links.Queries.GetLinks;

public class GetLinksQueryHandler : IRequestHandler<GetLinksQuery, IEnumerable<LinkDetailsResponse>>
{
    private readonly IAppDbContext _appDbContext;

    public GetLinksQueryHandler(IAppDbContext appDbContext)
    {
        _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
    }

    public async Task<IEnumerable<LinkDetailsResponse>> Handle(GetLinksQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<LinkDetailsResponse> links = await _appDbContext.Links
             .Include(l => l.LinkStatistic)
                .ThenInclude(st => st.Geolocation)
             .AsNoTracking()
             .Select(link => LinkDetailsResponse.MapToLInkDto(link))
             .ToArrayAsync();
        return links;
    }
}
