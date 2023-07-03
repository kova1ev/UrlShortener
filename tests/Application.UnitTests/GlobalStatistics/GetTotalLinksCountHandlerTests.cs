using Application.UnitTests.Utility;
using UrlShortener.Application.GlobalStatistics.Queries.GetTotalLinksCount;

namespace Application.UnitTests.GlobalStatistics;

public class GetTotalLinksCountHandlerTests
{
    [Fact]
    public async Task Should_return_SuccessResult()
    {
        var expectedTotalCount = SeedData.Links.Count; 
        var request = new GetTotalLinksCountQuery();

        await using var appDbContext = DbContextHelper.CreateContext();
        var handler = new GetTotalLinksCountHandler(appDbContext);

        //act
        var result = await handler.Handle(request, CancellationToken.None);

        //assert
        Assert.True(result.IsSuccess);
        Assert.True(result.HasValue);
        Assert.Equal(expectedTotalCount,result.Value);
    }
}