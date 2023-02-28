using MediatR;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Data;
using UrlShortener.Domain.Entity;

namespace UrlShortener.Application.Links.Queries.GetLinks;

public class GetLinksQueryHandler : IRequestHandler<GetLinksQuery, IEnumerable<Link>>
{
    private readonly AppDbContext _appDbContext;

    public GetLinksQueryHandler(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
    }

    public async Task<IEnumerable<Link>> Handle(GetLinksQuery request, CancellationToken cancellationToken)
    {
        var links = await _appDbContext.Links.Include(l => l.LinkInfo).AsNoTracking().ToArrayAsync(); ;
        return links;
    }
}
