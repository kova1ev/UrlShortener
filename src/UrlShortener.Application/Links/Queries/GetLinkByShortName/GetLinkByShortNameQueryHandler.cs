using MediatR;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Application.Common.Models.Links;
using UrlShortener.Application.Common.Result;
using UrlShortener.Data;
using UrlShortener.Domain.Entity;

namespace UrlShortener.Application.Links.Queries.GetLinkByShortName;

public class GetLinkByShortNameQueryHandler : IRequestHandler<GetLinkByShortNameQuery, Result<LinkDto>>
{
    private readonly AppDbContext _appDbContext;

    public GetLinkByShortNameQueryHandler(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
    }

    public async Task<Result<LinkDto>> Handle(GetLinkByShortNameQuery request, CancellationToken cancellationToken)
    {
        Link? link = await _appDbContext.Links
            .Include(l => l.LinkStatistic)
                .ThenInclude(st => st.Geolocation)
            .AsNoTracking().FirstOrDefaultAsync(l => l.Alias == request.Alias);
        if (link == null)
        {
            return Result<LinkDto>.Failure(new[] { "Link Not Found" });
        }

        return Result<LinkDto>.Success(LinkDto.MapToLInkDto(link));
    }
}
