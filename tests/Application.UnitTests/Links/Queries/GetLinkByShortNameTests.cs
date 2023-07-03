using Application.UnitTests.Utility;
using UrlShortener.Application.Common.Constants;
using UrlShortener.Application.Links.Queries.GetLinkByShortName;

namespace Application.UnitTests.Links.Queries;

public class GetLinkByShortNameTests
{
    public static IEnumerable<object[]> GoodAlias = new List<object[]>
    {
        new object[] { "aaa" },
        new object[] { "bbb" }
    };

    [Theory]
    [MemberData(nameof(GoodAlias))]
    public async Task GetLinkByShortName_Should_return_SuccessResult(string alias)
    {
        //arrange
        GetLinkByShortNameQuery request = new(alias);
        using var context = DbContextHelper.CreateContext();
        GetLinkByShortNameQueryHandler handler = new(context);

        //act
        var result = await handler.Handle(request, CancellationToken.None);
        //assert

        Assert.True(result.IsSuccess);
        Assert.True(result.HasValue);
        Assert.Empty(result.Errors);
        Assert.EndsWith(alias, result.Value.UrlShort);
    }

    [Theory]
    [InlineData("zzzzz")]
    [InlineData("fsdfdsf")]
    public async Task GetLinkByShortName_Should_return_FailureResult_WhenAliasIsNotExistingInDB(string shortName)
    {
        //arrange
        GetLinkByShortNameQuery request = new(shortName);
        using var context = DbContextHelper.CreateContext();
        GetLinkByShortNameQueryHandler handler = new(context);

        //act
        var result = await handler.Handle(request, CancellationToken.None);

        //assert
        Assert.False(result.IsSuccess);
        Assert.False(result.HasValue);
        Assert.NotEmpty(result.Errors);
        Assert.Single(result.Errors);
        Assert.Equal(LinkValidationErrorMessage.LinkNotExisting, result.Errors.FirstOrDefault());
    }
}