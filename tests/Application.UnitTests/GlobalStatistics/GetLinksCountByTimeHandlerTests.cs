using Application.UnitTests.Utility;
using UrlShortener.Application.GlobalStatistics.Queries.GetLinksCountByTime;

namespace Application.UnitTests.GlobalStatistics;

public class GetLinksCountByTimeHandlerTests
{
    [Fact]
    public async Task Should_return_SuccessResult_When_TimeParamsIsNullOrDefault()
    {
        int expectedCount = SeedData.Links.Count;
        var request = new GetLinksCountByTimeQuery(null, null);

        await using var dbContext = DbContextHelper.CreateContext();
        var handler = new GetLinksCountByTimeHandler(dbContext);

        //act
        var result = await handler.Handle(request, CancellationToken.None);

        //assert
        Assert.True(result.IsSuccess);
        Assert.True(result.HasValue);
        Assert.Equal(expectedCount, result.Value);
    }
}