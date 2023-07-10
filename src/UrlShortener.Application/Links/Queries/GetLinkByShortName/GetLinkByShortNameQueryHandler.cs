using MediatR;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Application.Common.Constants;
using UrlShortener.Application.Common.Domain.Links;
using UrlShortener.Application.Common.Result;
using UrlShortener.Application.Interfaces;

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
        var link = await _appDbContext.Links
            .Include(l => l.LinkStatistic)
                .ThenInclude(st => st!.Geolocation)
            .AsNoTracking().FirstOrDefaultAsync(l => l.Alias == request.Alias, cancellationToken);

        if (link == null)
        {
            return Result<LinkDetailsResponse>.Failure(new[] { LinkValidationErrorMessage.LinkNotExisting });
        }

        return Result<LinkDetailsResponse>.Success(LinkDetailsResponse.MapToLInkDto(link)!);
    }
}
