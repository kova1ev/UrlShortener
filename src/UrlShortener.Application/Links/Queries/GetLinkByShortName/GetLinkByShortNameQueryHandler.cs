using MediatR;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Application.Common.Models.Links;
using UrlShortener.Application.Common.Result;
using UrlShortener.Application.Interfaces;
using UrlShortener.Domain.Entity;

namespace UrlShortener.Application.Links.Queries.GetLinkByShortName;

public class GetLinkByShortNameQueryHandler : IRequestHandler<GetLinkByShortNameQuery, Result<LinkDetailsResponse>>
{
    private readonly IAppDbContext _appDbContext;

    public GetLinkByShortNameQueryHandler(IAppDbContext appDbContext)
    {
        _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
    }

    public async Task<Result<LinkDetailsResponse>> Handle(GetLinkByShortNameQuery request, CancellationToken cancellationToken)
    {
        Link? link = await _appDbContext.Links
            .Include(l => l.LinkStatistic)
                .ThenInclude(st => st.Geolocation)
            .AsNoTracking().FirstOrDefaultAsync(l => l.Alias == request.Alias);
        if (link == null)
        {
            return Result<LinkDetailsResponse>.Failure(new[] { "Link Not Found" });
        }

        return Result<LinkDetailsResponse>.Success(LinkDetailsResponse.MapToLInkDto(link));
    }
}
