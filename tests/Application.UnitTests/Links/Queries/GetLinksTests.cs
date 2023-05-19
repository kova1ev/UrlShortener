using Application.UnitTests.Utility;
using UrlShortener.Application.Common.Models.Links;
using UrlShortener.Application.Common.Result;
using UrlShortener.Application.Links.Queries.GetLinks;
using UrlShortener.Data;

namespace Application.UnitTests.Links.Queries;

public class GetLinksTests
{

    [Fact]
    public async Task Get_all_links()
    {
        //arrange
        using AppDbContext context = DbContextHepler.CreateContext();
        GetLinksQueryHandler handler = new(context);

        //act
        GetLinksQuery request = new();
        Result<IEnumerable<LinkDetailsResponse>> result = await handler.Handle(request, CancellationToken.None);

        //assert
        Assert.NotNull(result.Value);
        Assert.NotEmpty(result.Value);
        Assert.Equal(2, result.Value.Count());
    }
}
