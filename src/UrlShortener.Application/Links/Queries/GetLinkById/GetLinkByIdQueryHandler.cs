using MediatR;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Application.Common.Links;
using UrlShortener.Application.Common.Result;
using UrlShortener.Application.Links.Queries.GetLinkByShortName;
using UrlShortener.Data;

namespace UrlShortener.Application.Links.Queries.GetLinkById;

public class GetLinkByIdQueryHandler : IRequestHandler<GetLinkByIdQuery, Result<LinkDto>>
{
    private readonly AppDbContext _appDbContext;

    public GetLinkByIdQueryHandler(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
    }

    public async Task<Result<LinkDto>> Handle(GetLinkByIdQuery request, CancellationToken cancellationToken)
    {
        var link = await _appDbContext.Links.Include(l => l.LinkInfo).AsNoTracking().FirstOrDefaultAsync(l => l.Id == request.Id);

        if (link == null)
        {
            return Result<LinkDto>.Failure(new[] { "Link not found" });
        }

        return Result<LinkDto>.Success(LinkDto.MapToLInkDto(link));
    }
}
