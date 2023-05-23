using MediatR;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Application.Common.Models;
using UrlShortener.Application.Common.Models.Links;
using UrlShortener.Application.Common.Result;
using UrlShortener.Application.Interfaces;


namespace UrlShortener.Application.Links.Queries.GetLinks;

public class GetLinksQueryHandler : IRequestHandler<GetLinksQuery, Result<FilteredPagedData<LinkCompactResponse>>>
{
    private readonly IAppDbContext _appDbContext;

    public GetLinksQueryHandler(IAppDbContext appDbContext)
    {
        _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
    }

    public async Task<Result<FilteredPagedData<LinkCompactResponse>>> Handle(GetLinksQuery request, CancellationToken cancellationToken)
    {

        var list = _appDbContext.Links.AsNoTracking();


        // serch =>
        if (string.IsNullOrWhiteSpace(request.RequestParameters.Text) == false)
        {
            list = list.Where(l => EF.Functions.ILike(
                l.UrlAddress,
                $"%{request.RequestParameters.Text}%"));
        }

        // sort =>
        switch (request.RequestParameters.DateSort)
        {
            case DateSort.Asc:
                list = list.OrderBy(l => l.DateTimeCreated);
                break;
            default:
                list = list.OrderByDescending(l => l.DateTimeCreated);
                break;
        }

        // pagination  => 

        var links = await FilteredPagedData<LinkCompactResponse>.CreateFilteredPagedData(
            list.Select(link => LinkCompactResponse.MapFromLink(link)),
            request.RequestParameters.PageSize,
            request.RequestParameters.Page);

        return Result<FilteredPagedData<LinkCompactResponse>>.Success(links);
    }
}


