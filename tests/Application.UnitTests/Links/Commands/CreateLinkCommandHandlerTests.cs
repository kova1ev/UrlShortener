using Application.UnitTests.Utility;
using Microsoft.EntityFrameworkCore;
using Moq;
using UrlShortener.Application.Common.Constants;
using UrlShortener.Application.Common.Domain.Links;
using UrlShortener.Application.Interfaces;
using UrlShortener.Application.Links.Commands.CreateLink;

namespace Application.UnitTests.Links.Commands;

public class CreateLinkCommandHandlerTests
{
    private readonly string _existingUrlAddress = "https://github.com";
    private readonly string _hostUrl = "https://localhost:7072/r";
    private readonly string _newUrlAddress = "https://www.google.com/";
    private readonly Mock<ILinkService> _mockLinkService = new Mock<ILinkService>();

    [Fact]
    public async Task Should_return_SuccessResult_WithNewAliasAndNewUrlAddress()
    {
        var inputAlias = "goog";
        var shortUrl = $"{_hostUrl}/{inputAlias}";

        var request = new CreateLinkCommand(_newUrlAddress, inputAlias);
        var domainName = new Uri(request.UrlAddress!).Host;
        
        _mockLinkService.Setup(ls => ls.CreateShortUrl(It.IsAny<string>())).Returns(shortUrl);
        _mockLinkService.Setup(s => s.AliasIsBusy(It.IsAny<string>(), default)).ReturnsAsync(false);

        using var context = DbContextHelper.CreateContext();
        var handler = new CreateLinkCommandHandler(context, _mockLinkService.Object);
        var initialLinksCount = await context.Links.CountAsync();

        //act
        var result = await handler.Handle(request, CancellationToken.None);

        //assert
        Assert.True(result.IsSuccess);
        Assert.True(result.HasValue);
        Assert.Empty(result.Errors);
        Assert.True(initialLinksCount + 1 == await context.Links.CountAsync());

        var linkCreatedResponse = Assert.IsType<LinkCreatedResponse>(result.Value);
        Assert.Equal(shortUrl, linkCreatedResponse.ShortUrl);
        Assert.NotEqual(default, linkCreatedResponse.Id);

        var link = await context.Links
            .Include(l => l.LinkStatistic)
            .ThenInclude(ls => ls!.Geolocation)
            .FirstOrDefaultAsync(l => l.Id == linkCreatedResponse.Id);

        Assert.Equal(domainName, link?.LinkStatistic?.DomainName);
        Assert.Equal(inputAlias, link?.Alias);
        Assert.Equal(_newUrlAddress.TrimEnd('/'), link?.UrlAddress);
    }


    [Fact]
    public async Task Should_return_SuccessResult_WhenUrlAddressIsNew_and_WithoutInputAlias()
    {
        string? alias = null;
        var randomGeneratedAlias = "qazwsx";
        var shortUrl = $"{_hostUrl}/{randomGeneratedAlias}";

        var request = new CreateLinkCommand(_newUrlAddress, alias!);
        var domainName = new Uri(request.UrlAddress!).Host;
        
        _mockLinkService.Setup(s => s.GenerateAlias(default)).ReturnsAsync(randomGeneratedAlias);
        _mockLinkService.Setup(ls => ls.CreateShortUrl(It.IsAny<string>())).Returns(shortUrl);

        using var context = DbContextHelper.CreateContext();
        var handler = new CreateLinkCommandHandler(context, _mockLinkService.Object);
        var initialLinksCount = await context.Links.CountAsync();

        //act
        var result = await handler.Handle(request, CancellationToken.None);

        //assert
        Assert.True(result.IsSuccess);
        Assert.True(result.HasValue);
        Assert.Empty(result.Errors);
        
        var linkCreatedResponse = Assert.IsType<LinkCreatedResponse>(result.Value);
        Assert.True(initialLinksCount + 1 == await context.Links.CountAsync());
        Assert.Equal(shortUrl, linkCreatedResponse.ShortUrl);
        Assert.NotEqual(default, linkCreatedResponse.Id);

        var link = await context.Links
            .Include(l => l.LinkStatistic)
            .ThenInclude(ls => ls!.Geolocation)
            .FirstOrDefaultAsync(l => l.Id == linkCreatedResponse.Id);

        Assert.Equal(domainName, link?.LinkStatistic?.DomainName);
        Assert.Equal(randomGeneratedAlias, link?.Alias);
        Assert.Equal(_newUrlAddress.TrimEnd('/'), link?.UrlAddress);
    }


    [Fact]
    public async Task Should_return_SuccessResult_WhenUrlAddressIsExistingInDb_and_WithoutNewInputAlias()
    {
        string? alias = null;
        var request = new CreateLinkCommand(_existingUrlAddress, alias!);

        using var context = DbContextHelper.CreateContext();
        var handler = new CreateLinkCommandHandler(context, _mockLinkService.Object);
        var initialLinksCount = await context.Links.CountAsync();

        //act
        var result = await handler.Handle(request, CancellationToken.None);

        //assert
        Assert.True(result.IsSuccess);
        Assert.True(result.HasValue);
        Assert.Empty(result.Errors);
        Assert.True(initialLinksCount == await context.Links.CountAsync());
        
        var linkCreatedResponse = Assert.IsType<LinkCreatedResponse>(result.Value);
        Assert.NotEqual(default!, linkCreatedResponse.Id);
    }

    [Fact]
    public async Task Should_return_SuccessResult_WhenUrlAddressIsExistingInDb_and_WithNewInputAlias()
    {
        var inputAlias = "git";
        var shortUrl = $"{_hostUrl}/{inputAlias}";
        var request = new CreateLinkCommand(_existingUrlAddress, inputAlias);
        var domainName = new Uri(request.UrlAddress!).Host;
        
        _mockLinkService.Setup(s => s.AliasIsBusy(It.IsAny<string>(), default)).ReturnsAsync(false);
        _mockLinkService.Setup(s => s.CreateShortUrl(It.IsAny<string>())).Returns(shortUrl);

        using var context = DbContextHelper.CreateContext();
        var handler = new CreateLinkCommandHandler(context, _mockLinkService.Object);
        var initialLinksCount = await context.Links.CountAsync();

        //act
        var result = await handler.Handle(request, CancellationToken.None);

        //assert
        Assert.True(result.IsSuccess);
        Assert.True(result.HasValue);
        Assert.Empty(result.Errors);
        Assert.True(initialLinksCount + 1 == await context.Links.CountAsync());
        var linkCreatedResponse = Assert.IsType<LinkCreatedResponse>(result.Value);
        Assert.Equal(shortUrl, linkCreatedResponse.ShortUrl);
        Assert.NotEqual(default, linkCreatedResponse.Id);

        var link = await context.Links
            .Include(l => l.LinkStatistic)
            .ThenInclude(ls => ls!.Geolocation)
            .FirstOrDefaultAsync(l => l.Id == linkCreatedResponse.Id);

        Assert.Equal(domainName, link?.LinkStatistic?.DomainName);
        Assert.Equal(inputAlias, link?.Alias);
        Assert.Equal(_existingUrlAddress, link?.UrlAddress);
    }

    [Fact]
    public async Task Should_return_failureResult_WhenInputAliasIsTaken()
    {
        var existingAlias = "aaa";
        var request = new CreateLinkCommand(_newUrlAddress, existingAlias);
        
        _mockLinkService.Setup(s => s.AliasIsBusy(It.IsAny<string>(), default)).ReturnsAsync(true);

        using var context = DbContextHelper.CreateContext();
        var handler = new CreateLinkCommandHandler(context, _mockLinkService.Object);
        var initialLinksCount = await context.Links.CountAsync();

        //act
        var result = await handler.Handle(request, CancellationToken.None);

        //assert
        Assert.False(result.IsSuccess);
        Assert.False(result.HasValue);
        Assert.NotEmpty(result.Errors);
        Assert.True(initialLinksCount == await context.Links.CountAsync());
        Assert.Single(result.Errors);
        Assert.Equal(LinkValidationErrorMessage.AliasTaken, result.Errors.First());
    }
}