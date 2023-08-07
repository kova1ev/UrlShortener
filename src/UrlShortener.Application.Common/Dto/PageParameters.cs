namespace UrlShortener.Application.Common.Dto;

public abstract class PageParameters
{
    // todo move limit pages to appsetings??
    private const int MaxPageSize = 50;
    private const int DefaultPage = 1;

    private int _pageSize = 10;
    private int _page = 1;
    public virtual int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize || value < 1 ? _pageSize : value;
    }
    public virtual int Page
    {
        get => _page;
        set => _page = value < 1 ? DefaultPage : value;
    }
}
