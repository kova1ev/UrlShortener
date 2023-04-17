using Application.UnitTests.Utility;
using Microsoft.Extensions.Options;
using Moq;
using UrlShortener.Application.Common;
using UrlShortener.Application.Interfaces;
using UrlShortener.Application.Services;
using UrlShortener.Data;

namespace Application.UnitTests.Services;

public class LinkServiceTests
{
    private const string APP_URL = "https://localhost:7072";

    [Theory]
    [InlineData("aaa")]
    [InlineData("bbb")]
    public async Task Alias_busy(string alias)
    {
        //arrange 
        Mock<IAliasGenerator> moq = new Mock<IAliasGenerator>();
        IOptions<AppOptions> options = Options.Create(new AppOptions() { AppUrl = APP_URL });

        using AppDbContext context = DbContextHepler.CreateContext();
        LinkService linkService = new(context, moq.Object, options);
        //act
        bool result = await linkService.AliasIsBusy(alias);

        //assert
        Assert.True(result);
    }

    [Theory]
    [InlineData("qwerty")]
    [InlineData("dsfsdf")]
    public async Task Alias_is_not_busy(string alias)
    {
        //arrange 
        Mock<IAliasGenerator> moq = new Mock<IAliasGenerator>();
        IOptions<AppOptions> options = Options.Create(new AppOptions() { AppUrl = APP_URL });

        using AppDbContext context = DbContextHepler.CreateContext();
        LinkService linkService = new(context, moq.Object, options);
        //act
        bool result = await linkService.AliasIsBusy(alias);

        //assert
        Assert.False(result);
    }

    [Theory]
    [InlineData("test1")]
    [InlineData("qwerty")]
    public void Create_short_url_Success(string alias)
    {
        Mock<IAliasGenerator> moqDummy = new Mock<IAliasGenerator>();
        IOptions<AppOptions> options = Options.Create(new AppOptions() { AppUrl = APP_URL });

        using AppDbContext context = DbContextHepler.CreateContext();
        LinkService linkService = new(context, moqDummy.Object, options);
        //act
        string shortUrl = linkService.CreateShortUrl(alias);


        Assert.NotNull(shortUrl);
        Assert.NotEmpty(shortUrl);
        Assert.Equal("https://localhost:7072/r/" + alias, shortUrl);
    }

    [Fact]
    public void Create_short_url_with_Null_app_url_Failure()
    {
        string? appUrl = null;
        Mock<IAliasGenerator> moqDummy = new Mock<IAliasGenerator>();
        IOptions<AppOptions> op = Options.Create(new AppOptions() { AppUrl = appUrl });

        using AppDbContext context = DbContextHepler.CreateContext();
        LinkService linkService = new(context, moqDummy.Object, op);
        //act
        //assert
        Assert.Throws<ArgumentNullException>(() => linkService.CreateShortUrl("test"));
    }


    [Fact]
    public async Task Generate_alias_Success()
    {
        var returnAlias = "zzzz";
        //arrange
        var mockAliasGenerator = new Mock<IAliasGenerator>();
        mockAliasGenerator.Setup(g => g.GenerateAlias(4, 10)).Returns(returnAlias);
        IOptions<AppOptions> options = Options.Create(new AppOptions { AppUrl = APP_URL });

        using var context = DbContextHepler.CreateContext();
        var linkService = new LinkService(context, mockAliasGenerator.Object, options);

        //act
        string alias = await linkService.GenerateAlias();

        //assert
        Assert.NotNull(alias);
        Assert.NotEmpty(alias);
        Assert.Equal(returnAlias, alias);
    }

}
