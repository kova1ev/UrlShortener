using MediatR;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Application.Links.Queries.GetLinkByShortName;
using UrlShortener.Data;

namespace UrlShortener.Application.Links.Queries.GetLinkById;

public class GetLinkByIdQueryHandler : IRequestHandler<GetLinkByIdQuery, LinkDto>
{
    private readonly AppDbContext _appDbContext;

    public GetLinkByIdQueryHandler(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
    }

    public async Task<LinkDto> Handle(GetLinkByIdQuery request, CancellationToken cancellationToken)
    {
        var link = await _appDbContext.Links.Include(l => l.LinkInfo).AsNoTracking().FirstOrDefaultAsync(l => l.Id == request.Id);

        return new LinkDto(link);
    }
}
