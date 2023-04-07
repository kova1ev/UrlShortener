using MediatR;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Application.Common.Constants;
using UrlShortener.Application.Common.Models.Links;
using UrlShortener.Application.Common.Result;
using UrlShortener.Application.Interfaces;
using UrlShortener.Application.Links.Queries.GetLinkByShortName;


namespace UrlShortener.Application.Links.Queries.GetLinkById;

public class GetLinkByIdQueryHandler : IRequestHandler<GetLinkByIdQuery, Result<LinkDetailsResponse>>
{
    private readonly IAppDbContext _appDbContext;

    public GetLinkByIdQueryHandler(IAppDbContext appDbContext)
    {
        _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
    }

    public async Task<Result<LinkDetailsResponse>> Handle(GetLinkByIdQuery request, CancellationToken cancellationToken)
    {
        var link = await _appDbContext.Links
            .Include(l => l.LinkStatistic)
                .ThenInclude(st => st.Geolocation)
            .AsNoTracking().FirstOrDefaultAsync(l => l.Id == request.Id);

        if (link == null)
        {
            return Result<LinkDetailsResponse>.Failure(new[] { LinkValidationErrorMessage.LINK_NOT_EXISTING });
        }

        return Result<LinkDetailsResponse>.Success(LinkDetailsResponse.MapToLInkDto(link));
    }
}
