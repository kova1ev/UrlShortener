using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UrlShortener.Application.Common.Domain;
using UrlShortener.Application.Common.Domain.Links;
using UrlShortener.Application.Common.Result;
using UrlShortener.Application.Interfaces;
using UrlShortener.Entity;

namespace UrlShortener.Application.Links.Queries.GetLinks;

public class GetLinksQueryHandler : IRequestHandler<GetLinksQuery, Result<FilteredPagedData<LinkCompactResponse>>>
{
    private readonly IAppDbContext _appDbContext;

    public GetLinksQueryHandler(IAppDbContext appDbContext)
    {
        _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
    }

    public async Task<Result<FilteredPagedData<LinkCompactResponse>>> Handle(GetLinksQuery request,
        CancellationToken cancellationToken)
    {
        var links = _appDbContext.Links.AsNoTracking();

        links = SearchIntUrl(links, request.RequestParameters.Text);

        links = SortByDAteCreated(links, request.RequestParameters.DateSort);

        var pagedLinks = await FilteredPagedData<LinkCompactResponse>.CreateFilteredPagedData(
            links.Select(link => LinkCompactResponse.MapFromLink(link)),
            request.RequestParameters.PageSize,
            request.RequestParameters.Page,
            cancellationToken);

        return Result<FilteredPagedData<LinkCompactResponse>>.Success(pagedLinks);
    }

    // TODO search keywords
    private IQueryable<T> SearchIntUrl<T>(IQueryable<T> source, string? term) where T : Link
    {
        if (string.IsNullOrWhiteSpace(term) == false)
        {
            var splitSeparators = new char[] { ',', ' ', ';', ':', '/', '\\', '|', };
            var keywords = term?.Split(splitSeparators, StringSplitOptions.RemoveEmptyEntries)
                .Distinct()
                .ToArray();
            if (keywords != null && keywords.Length > 1)
            {
                BinaryExpression? binaryExpression = null;
                var parameter = Expression.Parameter(typeof(Link), "link");
                var urlAddressProperty = Expression.Property(parameter, "UrlAddress");
                var left = Expression.Call(urlAddressProperty, "Contains", Type.EmptyTypes,
                    Expression.Constant(keywords[0].ToLower()));
                for (int i = 1; i < keywords.Length; i++)
                {
                    string lowCaseTerm = keywords[i].ToLower();
                    var right = Expression.Call(urlAddressProperty, "Contains", Type.EmptyTypes,
                        Expression.Constant(lowCaseTerm));

                    if (binaryExpression == null)
                    {
                        binaryExpression = Expression.OrElse(left, right);
                    }
                    else
                    {
                        binaryExpression = Expression.OrElse(binaryExpression, right);
                    }
                }

                var lambda = Expression.Lambda<Func<Link, bool>>(binaryExpression, parameter);
                source = (IQueryable<T>)source.Where(lambda);
            }
            else
            {
                string keyWord = term.Trim().ToLower();
                source = source.Where(link => link.UrlAddress!.Contains(keyWord));
            }
        }

        return source;
    }

    private IQueryable<T> SortByDAteCreated<T>(IQueryable<T> source, DateSort dateSort) where T : Link
    {
        switch (dateSort)
        {
            case DateSort.Asc:
                source = source.OrderBy(l => l.DateTimeCreated);
                break;
            default:
                source = source.OrderByDescending(l => l.DateTimeCreated);
                break;
        }

        return source;
    }
}