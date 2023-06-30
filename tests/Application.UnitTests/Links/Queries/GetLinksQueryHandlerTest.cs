using Application.UnitTests.Utility;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Application.Common.Domain;
using UrlShortener.Application.Common.Domain.Links;
using UrlShortener.Application.Links.Queries.GetLinks;

namespace Application.UnitTests.Links.Queries;

public class GetLinksQueryHandlerTest
{
    [Fact]
    public async Task GetLinks_By_Only_Pagination()
    {
        var pageSize = 10;
        var page = 1;
        
        //arrange
        using var context = DbContextHepler.CreateContext();
        var iniLinksCount = await context.Links.CountAsync();
        
        LinksRequestParameters requestParameters = new() { Page = page, PageSize = pageSize };
        GetLinksQuery request = new(requestParameters);
        GetLinksQueryHandler handler = new(context);
        
        //act
        var result = await handler.Handle(
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
        int expectedCount = 1; 
        //arrange 
        LinksRequestParameters requestParameters = new()
        {
            Text = keyWord
        };

        GetLinksQuery getLinksQuery = new(requestParameters);

        using var appDbContext = DbContextHepler.CreateContext();

        GetLinksQueryHandler handler = new(appDbContext);
        //act
        var result =
            await handler.Handle(getLinksQuery, CancellationToken.None);

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