using MediatR;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Application.Common.Constants;
using UrlShortener.Application.Common.Domain.Links;
using UrlShortener.Application.Common.Result;
using UrlShortener.Application.Interfaces;


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
                .ThenInclude(st => st!.Geolocation)
            .AsNoTracking().FirstOrDefaultAsync(l => l.Id == request.Id,cancellationToken);

        if (link == null)
        {
            return Result<LinkDetailsResponse>.Failure(new[] { LinkValidationErrorMessage.LinkNotExisting });
        }

        return Result<LinkDetailsResponse>.Success(LinkDetailsResponse.MapToLInkDto(link)!);
    }
}
