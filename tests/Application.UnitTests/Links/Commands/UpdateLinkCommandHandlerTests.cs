using Application.UnitTests.Utility;
using Microsoft.EntityFrameworkCore;
using Moq;
using UrlShortener.Application.Common.Constants;
using UrlShortener.Application.Common.Result;
using UrlShortener.Application.Interfaces;
using UrlShortener.Application.Links.Commands.UpdateLink;
using UrlShortener.Domain.Entity;

namespace Application.UnitTests.Links.Commands;

public class UpdateLinkCommandHandlerTests
{
    private readonly string _hostUrl = "https://localhost:7072/r";
    private readonly Guid _linkId = new Guid("567BD1BF-6287-4331-A50E-82984DB0B97D");
    private readonly string _newUrlAddress = "https://ya.ru/";

    [Fact]
    public async Task Update_link_only_with_new_UrlAddress_Success()
    {
        //arrange
        string? newAlias = null;
        var request = new UpdateLinkCommand(_linkId, _newUrlAddress, newAlias);

        var mockLinkService = new Mock<ILinkService>();

        using var context = DbContextHepler.CreateContext();
        var handler = new UpdateLinkCommandHandler(context, mockLinkService.Object);

        //act
        Result result = await handler.Handle(request, CancellationToken.None);

        //assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Errors!);

        Link? link = await context.Links.Include(l => l.LinkStatistic).FirstOrDefaultAsync(l => l.Id == _linkId);
        //assert updated link
        Assert.NotNull(link);
        Assert.Equal(_newUrlAddress.TrimEnd('/').ToLower(), link.UrlAddress);
        Assert.Equal(new Uri(_newUrlAddress).Host, link.LinkStatistic?.DomainName);
        Assert.NotNull(link.Alias);
    }


    [Fact]
    public async Task Update_link_only_with_new_Alias_Success()
    {
        //arrange
        string? emptyUrlAddress = null;
        var newAlias = "banana";
        var request = new UpdateLinkCommand(_linkId, emptyUrlAddress, newAlias);

        var mockLinkService = new Mock<ILinkService>();
        mockLinkService.Setup(s => s.CreateShortUrl(It.IsAny<string>())).Returns($"{_hostUrl}/{newAlias}");

        using var context = DbContextHepler.CreateContext();
        var handler = new UpdateLinkCommandHandler(context, mockLinkService.Object);

        //act
        Result result = await handler.Handle(request, CancellationToken.None);

        //assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Errors!);

        Link? link = await context.Links.Include(l => l.LinkStatistic).FirstOrDefaultAsync(l => l.Id == _linkId);
        var shortUrl = $"{_hostUrl}/{newAlias}";
        //assert updated link
        Assert.NotNull(link);
        Assert.NotNull(link.UrlAddress);
        Assert.Equal(shortUrl, link.UrlShort);
        Assert.Equal(newAlias, link.Alias);
    }

    [Fact]
    public async Task Update_link_with_new_Alias_and_new_UrlAddress_Success()
    {
        //arrange
        var newAlias = "yayaya";
        var request = new UpdateLinkCommand(_linkId, _newUrlAddress, newAlias);

        var mockLinkService = new Mock<ILinkService>();
        mockLinkService.Setup(s => s.CreateShortUrl(It.IsAny<string>())).Returns($"{_hostUrl}/{newAlias}");

        using var context = DbContextHepler.CreateContext();
        var handler = new UpdateLinkCommandHandler(context, mockLinkService.Object);

        //act
        Result result = await handler.Handle(request, CancellationToken.None);

        //assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Errors!);

        Link? link = await context.Links.Include(l => l.LinkStatistic).FirstOrDefaultAsync(l => l.Id == _linkId);
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
    public async Task Try_Update_link_with_bad_id_Failure()
    {
        //arrange
        Guid linkId = default!;//  Guid.NewGuid();
        var request = new UpdateLinkCommand(linkId, null!, null!);

        var mockLinkService = new Mock<ILinkService>();
        using var context = DbContextHepler.CreateContext();
        var handler = new UpdateLinkCommandHandler(context, mockLinkService.Object);
        //act
        Result result = await handler.Handle(request, CancellationToken.None);

        //assert
        Assert.False(result.IsSuccess);
        Assert.NotEmpty(result.Errors!);
        Assert.True(result.Errors?.Count() == 1);
        Assert.Equal(LinkValidationErrorMessage.LINK_NOT_EXISTING, result.Errors?.First());
    }

    [Fact]
    public async Task Try_Update_link_with_existing_Alias_Failure()
    {
        //arrange
        string? emptyUrlAddress = null;
        var newAlias = "bbb"; // existing alias in db
        var request = new UpdateLinkCommand(_linkId, emptyUrlAddress!, newAlias);

        var mockLinkService = new Mock<ILinkService>();
        mockLinkService.Setup(s => s.AliasIsBusy(It.IsAny<string>())).ReturnsAsync(true);

        using var context = DbContextHepler.CreateContext();
        var handler = new UpdateLinkCommandHandler(context, mockLinkService.Object);
        //act
        Result result = await handler.Handle(request, CancellationToken.None);

        //assert
        Assert.False(result.IsSuccess);
        Assert.NotEmpty(result.Errors!);
        Assert.True(result.Errors?.Count() == 1);
        Assert.Equal(LinkValidationErrorMessage.ALIAS_TAKEN, result.Errors?.First());
    }
}
