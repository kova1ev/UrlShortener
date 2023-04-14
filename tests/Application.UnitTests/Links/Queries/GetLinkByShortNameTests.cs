using Application.UnitTests.Utility;
using UrlShortener.Application.Common.Constants;
using UrlShortener.Application.Common.Models.Links;
using UrlShortener.Application.Common.Result;
using UrlShortener.Application.Links.Queries.GetLinkByShortName;
using UrlShortener.Data;

namespace Application.UnitTests.Links.Queries;

public class GetLinkByShortNameTests
{

    [Theory]
    [InlineData("aaa")]
    [InlineData("bbb")]
    public async Task Get_link_by_short_name_Success(string shortName)
    {
        //arrange
        GetLinkByShortNameQuery request = new(shortName);
        using AppDbContext context = DbContextHepler.CreateContext();
        GetLinkByShortNameQueryHandler handler = new(context);

        //act
        Result<LinkDetailsResponse> result = await handler.Handle(request, CancellationToken.None);
        //assert

        Assert.True(result.IsSuccess);
        Assert.True(result.HasValue);
        Assert.Empty(result.Errors!);
        Assert.EndsWith(shortName, result.Value.UrlShort);
    }

    [Theory]
    [InlineData("zzzzz")]
    [InlineData("fsdfdsf")]
    public async Task Get_link_by_bad_short_name_Failure(string shortName)
    {
        //arrange
        GetLinkByShortNameQuery request = new(shortName);
        using AppDbContext context = DbContextHepler.CreateContext();
        GetLinkByShortNameQueryHandler handler = new(context);

        //act
        Result<LinkDetailsResponse> result = await handler.Handle(request, CancellationToken.None);

        //assert
        Assert.False(result.IsSuccess);
        Assert.False(result.HasValue);
        Assert.NotEmpty(result.Errors!);
        Assert.Equal(1, result.Errors.Count());
        Assert.Equal(LinkValidationErrorMessage.LINK_NOT_EXISTING, result.Errors.FirstOrDefault());
    }

}
