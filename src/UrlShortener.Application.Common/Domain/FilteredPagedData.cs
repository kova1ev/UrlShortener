using Microsoft.EntityFrameworkCore;

namespace UrlShortener.Application.Common.Domain;

public class FilteredPagedData<T>
{
    public int CurrentPage { get; private set; }
    public int PageSize { get; private set; }
    public int TotalPages { get; private set; }
    public int TotalCount { get; private set; }
    public IEnumerable<T> Data { get; private set; }

    public FilteredPagedData(IEnumerable<T> data, int totalCount, int pageSize, int page)
    {
        Data = data;
        TotalCount = totalCount;
        CurrentPage = page;
        PageSize = pageSize;
        TotalPages = (int)Math.Ceiling((double)TotalCount / pageSize);
    }


    public static async Task<FilteredPagedData<T>> CreateFilteredPagedData(IQueryable<T> source, int pageSize, int page,
        CancellationToken cancellationToken = default)
    {
        var count = await source.CountAsync(cancellationToken);

        var result = await source
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToArrayAsync(cancellationToken);

        return new FilteredPagedData<T>(result, count, pageSize, page);
    }
}