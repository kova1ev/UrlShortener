using Application.UnitTests.Utility;
using UrlShortener.Application.Common.Domain.Links;
using UrlShortener.Application.Links.Queries.GetMostRedirectedLinks;

namespace Application.UnitTests.Links.Queries;

public class GetMostRedirectedLinksQueryHandlerTests
{
    [Fact]
    public async Task Should_return_SuccessResult()
    {
        var mostClickableLink =
          SeedData.Links.OrderByDescending(l => (l.LinkStatistic!.Clicks, l.DateTimeCreated)).First();
        //arrange 
        var request = new GetMostRedirectedLinksQuery();

        await using var dbContext = DbContextHelper.CreateContext();
        var handler = new GetMostRedirectedLinksQueryHandler(dbContext);

        //act
        var result = await handler.Handle(request, CancellationToken.None);

        //assert 
        Assert.True(result.IsSuccess);
        Assert.True(result.HasValue);
        Assert.NotNull(result.Value);

        var links = Assert.IsAssignableFrom<IEnumerable<LinkCompactResponse>>(result.Value).ToArray();

        Assert.NotNull(links);
        Assert.Equal(mostClickableLink.Id, links.First().Id);
        Assert.Equal(SeedData.Links.Count, links.Length);
    }
}