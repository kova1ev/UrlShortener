using MediatR;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Application.Interfaces;
using UrlShortener.Application.Links.Queries.GetLinkByShortName;
using UrlShortener.Domain.Entity;

namespace UrlShortener.Application.Links.Queries.GetLinkById;

public class GetLinkByIdQueryHandler : IRequestHandler<GetLinkByIdQuery, Link?>
{
    private readonly IAppDbContext _appDbContext;

    public GetLinkByIdQueryHandler(IAppDbContext appDbContext)
    {
        _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
    }

    public async Task<Link?> Handle(GetLinkByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _appDbContext.Links.AsNoTracking().FirstOrDefaultAsync(l => l.Id == request.Id);

        return result;
    }
}
