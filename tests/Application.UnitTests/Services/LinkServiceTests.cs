using Application.UnitTests.Utility;
using Microsoft.Extensions.Options;
using Moq;
using UrlShortener.Application.Common;
using UrlShortener.Application.Interfaces;
using UrlShortener.Application.Services;

namespace Application.UnitTests.Services;

public class LinkServiceTests
{
    private const string FakeAppUrl = "https://localhost:7072";
    private readonly IOptions<AppOptions> _appOptions;
    private readonly Mock<IAliasGenerator> _mockAliasGenerator;
    
    public LinkServiceTests()
    {
        _mockAliasGenerator = new Mock<IAliasGenerator>();
        _appOptions = Options.Create(new AppOptions() { AppUrl = FakeAppUrl });
    }

    public static IEnumerable<object[]> BusyAlias = SeedData.Links.Select(l => new []{ l.Alias}).ToList()!;

    public static IEnumerable<object[]> FreeAlias = new List<object[]>()
    {
        new object[] { "qwerty" },
        new object[] { "dsfsdf" }
    };
    
    [Theory]
    [MemberData(nameof(BusyAlias))]
    public async Task AliasIsBusy_Should_return_False(string alias)
    {
        //arrange
        using var context = DbContextHelper.CreateContext();
        ILinkService linkService = new LinkService(context, _mockAliasGenerator.Object, _appOptions);
        
        //act
        var result = await linkService.AliasIsBusy(alias);

        //assert
        Assert.True(result);
    }

    [Theory]
    [MemberData(nameof(FreeAlias))]
    public async Task AliasIsBusy_Should_return_True(string alias)
    {
        //arrange
        using var context = DbContextHelper.CreateContext();
        ILinkService linkService = new LinkService(context, _mockAliasGenerator.Object, _appOptions);
        
        //act
        var result = await linkService.AliasIsBusy(alias);

        //assert
        Assert.False(result);
    }

    [Theory]
    [MemberData(nameof(FreeAlias))]
    public void CreateShortUrl_Should_return_ValidShortUrl(string alias)
    {
        //arrange
        string urlPattern = "https://localhost:7072/r/" + alias;
        using var context = DbContextHelper.CreateContext();
        ILinkService linkService = new LinkService(context, _mockAliasGenerator.Object, _appOptions);

        //act
        var shortUrl = linkService.CreateShortUrl(alias);

        //assert
        Assert.NotNull(shortUrl);
        Assert.NotEmpty(shortUrl);
        Assert.Equal(urlPattern, shortUrl);
    }

    [Fact]
    public void CreateShortUrl_Should_throw_ArgumentNullException_when_AppUrlIsNull()
    {
        //arrange
        string randomAlias = "test";
        _appOptions.Value.AppUrl = null;
        using var context = DbContextHelper.CreateContext();
        ILinkService linkService = new LinkService(context, _mockAliasGenerator.Object, _appOptions);
        
        //assert
        Assert.Throws<ArgumentNullException>(() => linkService.CreateShortUrl(randomAlias));
    }

    [Fact]
    public async Task GenerateAlias_Should_return_Alias()
    {
        var returnAlias = "zzzz";
        //arrange
        _mockAliasGenerator.Setup(g => g.GenerateAlias(4, 10)).Returns(returnAlias);
        
        using var context = DbContextHelper.CreateContext();
        ILinkService linkService = new LinkService(context, _mockAliasGenerator.Object, _appOptions);

        //act
        var alias = await linkService.GenerateAlias();

        //assert
        Assert.NotNull(alias);
        Assert.NotEmpty(alias);
        Assert.Equal(returnAlias, alias);
    }
}