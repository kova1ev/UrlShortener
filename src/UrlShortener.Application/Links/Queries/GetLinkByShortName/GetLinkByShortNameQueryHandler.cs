using MediatR;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Application.Common.Links;
using UrlShortener.Data;

namespace UrlShortener.Application.Links.Queries.GetLinkByShortName;

public class GetLinkByShortNameQueryHandler : IRequestHandler<GetLinkByShortNameQuery, LinkDto>
{
    private readonly AppDbContext _appDbContext;

    public GetLinkByShortNameQueryHandler(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
    }

    public async Task<LinkDto> Handle(GetLinkByShortNameQuery request, CancellationToken cancellationToken)
    {
        var result = await _appDbContext.Links.Include(l => l.LinkInfo).AsNoTracking().FirstOrDefaultAsync(l => l.Alias == request.Alias);

        return LinkDto.MapToLInkDto(result);
    }
}
