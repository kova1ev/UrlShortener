using MediatR;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Application.Common.Links;
using UrlShortener.Data;

namespace UrlShortener.Application.Links.Queries.GetLinks;

public class GetLinksQueryHandler : IRequestHandler<GetLinksQuery, IEnumerable<LinkDto>>
{
    private readonly AppDbContext _appDbContext;

    public GetLinksQueryHandler(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
    }

    public async Task<IEnumerable<LinkDto>> Handle(GetLinksQuery request, CancellationToken cancellationToken)
    {
        var links = await _appDbContext.Links.Include(l => l.LinkInfo).AsNoTracking()
            .Select(link => LinkDto.MapToLInkDto(link))
            .ToArrayAsync();
        return links;
    }
}
