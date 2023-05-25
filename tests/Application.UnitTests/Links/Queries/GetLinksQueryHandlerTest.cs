using Application.UnitTests.Utility;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Application.Common.Models;
using UrlShortener.Application.Common.Models.Links;
using UrlShortener.Application.Common.Result;
using UrlShortener.Application.Links.Queries.GetLinks;
using UrlShortener.Data;

namespace Application.UnitTests.Links.Queries;

public class GetLinksQueryHandlerTest
{
    [Fact]
    public async Task GetLinks_By_Only_Pagination()
    {
        //arrange
        using AppDbContext context = DbContextHepler.CreateContext();
        int iniLinksCount = await context.Links.CountAsync();
        int page = 1;
        int pageSize = 10;
        LinksRequestParameters requestParameters = new() { Page = page, PageSize = pageSize };
        GetLinksQuery request = new(requestParameters);

        GetLinksQueryHandler handler = new(context);
        //act

        Result<FilteredPagedData<LinkCompactResponse>> result = await handler.Handle(
            request, CancellationToken.None);

        //assert
        Assert.NotNull(result.Value);
        Assert.True(result.IsSuccess);
        Assert.True(result.HasValue);

        var data = Assert.IsType<FilteredPagedData<LinkCompactResponse>>(result.Value);

        Assert.NotNull(data.Data);
        Assert.Equal(iniLinksCount, data.Data.ToArray().Length);
        Assert.Equal(iniLinksCount, data.TotalCount);
        Assert.Equal(page, data.CurrentPage);
        Assert.Equal(pageSize, data.PageSize);
        Assert.Equal(1, data.TotalPages);
    }

    [Theory]
    [InlineData("git")]
    [InlineData("leet")]
    public async Task Should_return_result_by_search(string keyWord)
    {
        //arrange 
        LinksRequestParameters requestParameters = new();
        requestParameters.Text = keyWord;

        GetLinksQuery getLinksQuery = new(requestParameters);

        using AppDbContext appDbContext = DbContextHepler.CreateContext();

        GetLinksQueryHandler handler = new(appDbContext);
        //act
        Result<FilteredPagedData<LinkCompactResponse>> result = await handler.Handle(getLinksQuery, CancellationToken.None);

        //assert

        Assert.NotNull(result.Value);
        Assert.True(result.IsSuccess);
        Assert.True(result.HasValue);
        Assert.Empty(result.Errors);

        var data = Assert.IsType<FilteredPagedData<LinkCompactResponse>>(result.Value);

        Assert.NotNull(data.Data);
        Assert.Single(data.Data.ToArray());
        Assert.Equal(1, data.TotalCount);
        Assert.Equal(1, data.CurrentPage);
        Assert.Equal(1, data.TotalPages);
    }
}
