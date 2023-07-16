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

    [Fact]
    public async Task Should_return_TotalCount_SuccessResult()
    {
        int expectedCount = SeedData.Links.Count;
        DateOnly d1 = DateOnly.FromDateTime(DateTime.MinValue);
        DateOnly d2 = DateOnly.FromDateTime(DateTime.MaxValue);
        var request = new GetLinksCountByTimeQuery(d1, d2);

        await using var context = DbContextHelper.CreateContext();
        var handler = new GetLinksCountByTimeHandler(context);

        var result = await handler.Handle(request, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.True(result.HasValue);
        Assert.Equal(expectedCount, result.Value);
    }
    
    [Fact]
    public async Task Should_return_TotalCount_SuccessResult1()
    {
        int expectedCount = 0;
        DateOnly d1 = DateOnly.FromDateTime(DateTime.MinValue);
        DateOnly d2 = DateOnly.FromDateTime(DateTime.UtcNow.Subtract(TimeSpan.FromDays(1)));
        var request = new GetLinksCountByTimeQuery(d1, d2);

        await using var context = DbContextHelper.CreateContext();
        var handler = new GetLinksCountByTimeHandler(context);

        var result = await handler.Handle(request, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.True(result.HasValue);
        Assert.Equal(0, result.Value);
    }
}