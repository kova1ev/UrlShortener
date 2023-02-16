using MediatR;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Application.Interfaces;
using UrlShortener.Domain.Entity;

namespace UrlShortener.Application.Links.Queries.GetLinks;

public class GetLinksQueryHandler : IRequestHandler<GetLinksQuery, IEnumerable<Link>>
{
    private readonly IAppDbContext _appDbContext;

    public GetLinksQueryHandler(IAppDbContext appDbContext)
    {
        _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
    }

    public async Task<IEnumerable<Link>> Handle(GetLinksQuery request, CancellationToken cancellationToken)
    {
        var links = await _appDbContext.Links.ToArrayAsync();
        return links;
    }
}
