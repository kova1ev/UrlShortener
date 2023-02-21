using MediatR;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Application.Interfaces;
using UrlShortener.Domain.Entity;

namespace UrlShortener.Application.Links.Queries.GetLinkByShortName;

public class GetLinkByShortNameQueryHandler : IRequestHandler<GetLinkByShortNameQuery, Link?>
{
    private readonly IAppDbContext _appDbContext;

    public GetLinkByShortNameQueryHandler(IAppDbContext appDbContext)
    {
        _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
    }

    public async Task<Link?> Handle(GetLinkByShortNameQuery request, CancellationToken cancellationToken)
    {
        var result = await _appDbContext.Links.Include(l => l.LinkInfo).AsNoTracking().FirstOrDefaultAsync(l => l.Alias == request.ShortName);

        return result;
    }
}

