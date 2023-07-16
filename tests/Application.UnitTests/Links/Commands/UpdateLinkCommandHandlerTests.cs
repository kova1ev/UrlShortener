using Application.UnitTests.Utility;
using Microsoft.EntityFrameworkCore;
using Moq;
using UrlShortener.Application.Common.Constants;
using UrlShortener.Application.Interfaces;
using UrlShortener.Application.Links.Commands.UpdateLink;

namespace Application.UnitTests.Links.Commands;

public class UpdateLinkCommandHandlerTests
{
    private readonly string _hostUrl = "https://localhost:7072/r";
    private readonly Guid _linkId = new("567BD1BF-6287-4331-A50E-82984DB0B97D");
    private readonly string _newUrlAddress = "https://ya.ru/";
    private readonly Mock<ILinkService> _mockLinkService = new Mock<ILinkService>();

    [Fact]
    public async Task UpdateLink_Should_return_SuccessResult_WithNewUrlAddress_only()
    {
        //arrange
        string? newAlias = null;
        var request = new UpdateLinkCommand(_linkId, _newUrlAddress, newAlias);

        using var context = DbContextHelper.CreateContext();
        var handler = new UpdateLinkCommandHandler(context, _mockLinkService.Object);

        //act
        var result = await handler.Handle(request, CancellationToken.None);

        //assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Errors);

        var link = await context.Links.Include(l => l.LinkStatistic).FirstOrDefaultAsync(l => l.Id == _linkId);
        //assert updated link
        Assert.NotNull(link);
        Assert.Equal(_newUrlAddress.TrimEnd('/').ToLower(), link.UrlAddress);
        Assert.Equal(new Uri(_newUrlAddress).Host, link.LinkStatistic?.DomainName);
        Assert.NotNull(link.Alias);
    }


    [Fact]
    public async Task UpdateLink_Should_return_SuccessResult_WithNewAlias_only()
    {
        //arrange
        string? emptyUrlAddress = null;
        var newAlias = "banana";
        var request = new UpdateLinkCommand(_linkId, emptyUrlAddress, newAlias);

        _mockLinkService.Setup(s => s.CreateShortUrl(It.IsAny<string>())).Returns($"{_hostUrl}/{newAlias}");

        using var context = DbContextHelper.CreateContext();
        var handler = new UpdateLinkCommandHandler(context, _mockLinkService.Object);

        //act
        var result = await handler.Handle(request, CancellationToken.None);

        //assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Errors);

        var link = await context.Links.Include(l => l.LinkStatistic).FirstOrDefaultAsync(l => l.Id == _linkId);
        var shortUrl = $"{_hostUrl}/{newAlias}";
        //assert updated link
        Assert.NotNull(link);
        Assert.NotNull(link.UrlAddress);
        Assert.Equal(shortUrl, link.UrlShort);
        Assert.Equal(newAlias, link.Alias);
    }

    [Fact]
    public async Task UpdateLink_Should_return_SuccessResult_WhenAliasAndUrlAddressIsNew()
    {
        //arrange
        var newAlias = "yayaya";
        var request = new UpdateLinkCommand(_linkId, _newUrlAddress, newAlias);

        _mockLinkService.Setup(s => s.CreateShortUrl(It.IsAny<string>())).Returns($"{_hostUrl}/{newAlias}");

        using var context = DbContextHelper.CreateContext();
        var handler = new UpdateLinkCommandHandler(context, _mockLinkService.Object);

        //act
        var result = await handler.Handle(request, CancellationToken.None);

        //assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Errors);

        var link = await context.Links.Include(l => l.LinkStatistic).FirstOrDefaultAsync(l => l.Id == _linkId);
        var shortUrl = $"{_hostUrl}/{newAlias}";
        //assert updated link
        Assert.NotNull(link);
        Assert.NotNull(link.Alias);
        Assert.NotNull(link.UrlAddress);
        Assert.Equal(shortUrl, link.UrlShort);
        Assert.Equal(newAlias, link.Alias);
        Assert.Equal(_newUrlAddress.TrimEnd('/').ToLower(), link.UrlAddress);
        Assert.Equal(new Uri(_newUrlAddress).Host, link.LinkStatistic?.DomainName);
    }

    [Fact]
    public async Task UpdateLink_Should_return_FailureResult_WhenIdIsInvalid()
    {
        //arrange
        Guid linkId = default!; //  Guid.NewGuid();
        var request = new UpdateLinkCommand(linkId, null!, null!);

        using var context = DbContextHelper.CreateContext();
        var handler = new UpdateLinkCommandHandler(context, _mockLinkService.Object);
        //act
        var result = await handler.Handle(request, CancellationToken.None);

        //assert
        Assert.False(result.IsSuccess);
        Assert.NotEmpty(result.Errors);
        Assert.Single(result.Errors);
        Assert.Equal(LinkValidationErrorMessage.LinkNotExisting, result.Errors.First());
    }

    [Fact]
    public async Task UpdateLink_Should_return_FailureResult_WhenAliasIsTaken()
    {
        //arrange
        string? emptyUrlAddress = null;
        var newAlias = "bbb"; // existing alias in db
        var request = new UpdateLinkCommand(_linkId, emptyUrlAddress!, newAlias);

        _mockLinkService.Setup(s => s.AliasIsBusy(It.IsAny<string>(), default)).ReturnsAsync(true);

        using var context = DbContextHelper.CreateContext();
        var handler = new UpdateLinkCommandHandler(context, _mockLinkService.Object);
        //act
        var result = await handler.Handle(request, CancellationToken.None);

        //assert
        Assert.False(result.IsSuccess);
        Assert.NotEmpty(result.Errors);
        Assert.Single(result.Errors);
        Assert.Equal(LinkValidationErrorMessage.AliasTaken, result.Errors.First());
    }
}