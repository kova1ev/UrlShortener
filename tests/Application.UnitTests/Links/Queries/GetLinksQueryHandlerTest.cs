using Application.UnitTests.Utility;
using UrlShortener.Application.Common.Dto;
using UrlShortener.Application.Links.Queries.GetLinks;
using UrlShortener.Application.Responses;

namespace Application.UnitTests.Links.Queries;

public class GetLinksQueryHandlerTest
{
    [Fact]
    public async Task GetLinks_Should_return_SuccessResult_WhenRequestParamsIsDefault()
    {
        //arrange
        var pageSize = 10;
        var page = 1;
        var expectedPageCount = 1;
        LinksRequestParameters requestParameters = new() { Page = page, PageSize = pageSize };
        GetLinksQuery request = new(requestParameters);

        using var context = DbContextHelper.CreateContext();
        var iniLinksCount = SeedData.Links.Count;

        GetLinksQueryHandler handler = new(context);

        //act
        var result = await handler.Handle(request, CancellationToken.None);

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
        Assert.Equal(expectedPageCount, data.TotalPages);
    }

    [Fact]
    public async Task GetLinks_Should_return_SuccessResult_WhenRequestParamsIsCustom()
    {
        //arrange
        var pageSize = 1;
        var page = 1;
        var expectedPageCount = 2;
        LinksRequestParameters requestParameters = new() { Page = page, PageSize = pageSize };
        GetLinksQuery request = new(requestParameters);

        using var context = DbContextHelper.CreateContext();
        var iniLinksCount = SeedData.Links.Count;

        GetLinksQueryHandler handler = new(context);

        //act
        var result = await handler.Handle(request, CancellationToken.None);

        //assert
        Assert.NotNull(result.Value);
        Assert.True(result.IsSuccess);
        Assert.True(result.HasValue);

        var data = Assert.IsType<FilteredPagedData<LinkCompactResponse>>(result.Value);

        Assert.NotNull(data.Data);
        Assert.Equal(pageSize, data.Data.ToArray().Length);
        Assert.Equal(iniLinksCount, data.TotalCount);
        Assert.Equal(page, data.CurrentPage);
        Assert.Equal(pageSize, data.PageSize);
        Assert.Equal(expectedPageCount, data.TotalPages);
    }

    [Theory]
    [InlineData("git")]
    [InlineData("leet")]
    public async Task Should_return_SuccessResult_by_search(string keyWord)
    {
        //arrange 
        int expectedCount = 1;
        LinksRequestParameters requestParameters = new()
        {
            Text = keyWord
        };
        GetLinksQuery getLinksQuery = new(requestParameters);

        using var appDbContext = DbContextHelper.CreateContext();
        GetLinksQueryHandler handler = new(appDbContext);

        //act
        var result = await handler.Handle(getLinksQuery, CancellationToken.None);

        //assert

        Assert.NotNull(result.Value);
        Assert.True(result.IsSuccess);
        Assert.True(result.HasValue);
        Assert.Empty(result.Errors);

        var data = Assert.IsType<FilteredPagedData<LinkCompactResponse>>(result.Value);

        Assert.NotNull(data.Data);
        Assert.Single(data.Data.ToArray());
        Assert.Equal(expectedCount, data.TotalCount);
        Assert.Equal(expectedCount, data.CurrentPage);
        Assert.Equal(expectedCount, data.TotalPages);
    }
}