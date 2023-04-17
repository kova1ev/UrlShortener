﻿using Application.UnitTests.Utility;
using Microsoft.EntityFrameworkCore;
using Moq;
using UrlShortener.Application.Common.Constants;
using UrlShortener.Application.Common.Models.Links;
using UrlShortener.Application.Common.Result;
using UrlShortener.Application.Interfaces;
using UrlShortener.Application.Links.Commands.CreateLink;

namespace Application.UnitTests.Links.Commands;

public class CreateLinkCommandHandlerTests
{
    private readonly string _hostUrl = "https://localhost:7072/r";
    private readonly string _newUrlAddress = "https://www.google.com/";
    private readonly string _existingUrlAddress = "https://github.com/";

    [Fact]
    public async Task Create_link_with_input_Alias_and_new_UrlAddress_Success()
    {
        var inputAlias = "goog";
        var shortUrl = $"{_hostUrl}/{inputAlias}";

        var request = new CreateLinkCommand(_newUrlAddress, inputAlias);
        string domainName = new Uri(request.UrlAddress!).Host;

        var mockLinkService = new Mock<ILinkService>();
        mockLinkService.Setup(ls => ls.CreateShortUrl(It.IsAny<string>())).Returns(shortUrl);
        mockLinkService.Setup(s => s.AliasIsBusy(It.IsAny<string>())).ReturnsAsync(false);

        using var context = DbContextHepler.CreateContext();
        var handler = new CreateLinkCommandHandler(context, mockLinkService.Object);
        int initialLinksCount = await context.Links.CountAsync();

        //act
        Result<LinkCreatedResponse> result = await handler.Handle(request, CancellationToken.None);

        //assert
        Assert.True(result.IsSuccess);
        Assert.True(result.HasValue);
        Assert.Empty(result.Errors!);
        Assert.IsType<LinkCreatedResponse>(result.Value);
        Assert.True(initialLinksCount + 1 == await context.Links.CountAsync());

        LinkCreatedResponse linkCreatedResponse = result.Value;
        Assert.Equal(shortUrl, linkCreatedResponse.ShortUrl);
        Assert.NotEqual(default, linkCreatedResponse.Id);

        var link = await context.Links
            .Include(l => l.LinkStatistic)
            .ThenInclude(ls => ls.Geolocation)
            .FirstOrDefaultAsync(l => l.Id == linkCreatedResponse.Id);

        Assert.Equal(domainName, link.LinkStatistic.DomainName);
        Assert.Equal(inputAlias, link.Alias);

    }


    [Fact]
    public async Task Create_link_without_input_Alias_and_with_new_UrlAddress_Success()
    {
        string? alias = null;
        var randomGeneratedAlias = "qazwsx";
        var shortUrl = $"{_hostUrl}/{randomGeneratedAlias}";

        var request = new CreateLinkCommand(_newUrlAddress, alias!);
        string domainName = new Uri(request.UrlAddress!).Host;

        var mockLinkService = new Mock<ILinkService>();
        mockLinkService.Setup(s => s.GenerateAlias()).ReturnsAsync(randomGeneratedAlias);
        mockLinkService.Setup(ls => ls.CreateShortUrl(It.IsAny<string>())).Returns(shortUrl);

        using var context = DbContextHepler.CreateContext();
        var handler = new CreateLinkCommandHandler(context, mockLinkService.Object);
        int initialLinksCount = await context.Links.CountAsync();

        //act
        Result<LinkCreatedResponse> result = await handler.Handle(request, CancellationToken.None);

        //assert
        Assert.True(result.IsSuccess);
        Assert.True(result.HasValue);
        Assert.Empty(result.Errors!);
        LinkCreatedResponse linkCreatedResponse = Assert.IsType<LinkCreatedResponse>(result.Value);
        Assert.True(initialLinksCount + 1 == await context.Links.CountAsync());
        Assert.Equal(shortUrl, linkCreatedResponse.ShortUrl);
        Assert.NotEqual(default, linkCreatedResponse.Id);

        var link = await context.Links
          .Include(l => l.LinkStatistic)
          .ThenInclude(ls => ls.Geolocation)
          .FirstOrDefaultAsync(l => l.Id == linkCreatedResponse.Id);

        Assert.Equal(domainName, link.LinkStatistic.DomainName);
        Assert.Equal(randomGeneratedAlias, link.Alias);
    }


    [Fact]
    public async Task Try_create_link_with_existing_UrlAddress_in_db_without_new_input_alias_Success()
    {
        string? alias = null; ;
        var request = new CreateLinkCommand(_existingUrlAddress, alias!);

        var mockLinkService = new Mock<ILinkService>();

        using var context = DbContextHepler.CreateContext();
        var handler = new CreateLinkCommandHandler(context, mockLinkService.Object);
        int initialLinksCount = await context.Links.CountAsync();

        //act
        Result<LinkCreatedResponse> result = await handler.Handle(request, CancellationToken.None);

        //assert
        Assert.True(result.IsSuccess);
        Assert.True(result.HasValue);
        Assert.Empty(result.Errors!);
        Assert.True(initialLinksCount == await context.Links.CountAsync());
        LinkCreatedResponse linkCreatedResponse = Assert.IsType<LinkCreatedResponse>(result.Value);
        Assert.NotEqual(default!, linkCreatedResponse.Id);
    }

    [Fact]
    public async Task Create_link_with_existing_UrlAddress_in_db_with_new_input_alias_Success()
    {
        var inputAlias = "git";
        var shortUrl = $"{_hostUrl}/{inputAlias}";
        var request = new CreateLinkCommand(_existingUrlAddress, inputAlias!);
        string domainName = new Uri(request.UrlAddress!).Host;

        var mockLinkService = new Mock<ILinkService>();
        mockLinkService.Setup(s => s.AliasIsBusy(It.IsAny<string>())).ReturnsAsync(false);
        mockLinkService.Setup(s => s.CreateShortUrl(It.IsAny<string>())).Returns(shortUrl);

        using var context = DbContextHepler.CreateContext();
        var handler = new CreateLinkCommandHandler(context, mockLinkService.Object);
        int initialLinksCount = await context.Links.CountAsync();

        //act
        Result<LinkCreatedResponse> result = await handler.Handle(request, CancellationToken.None);

        //assert
        Assert.True(result.IsSuccess);
        Assert.True(result.HasValue);
        Assert.Empty(result.Errors!);
        Assert.True(initialLinksCount + 1 == await context.Links.CountAsync());
        LinkCreatedResponse linkCreatedResponse = Assert.IsType<LinkCreatedResponse>(result.Value);
        Assert.Equal(shortUrl, linkCreatedResponse.ShortUrl);
        Assert.NotEqual(default, linkCreatedResponse.Id);

        var link = await context.Links
            .Include(l => l.LinkStatistic)
            .ThenInclude(ls => ls.Geolocation)
            .FirstOrDefaultAsync(l => l.Id == linkCreatedResponse.Id);

        Assert.Equal(domainName, link.LinkStatistic?.DomainName);
        Assert.Equal(inputAlias, link.Alias);
    }

    [Fact]
    public async Task Try_Create_link_with_existing_Alias_Failure()
    {
        var existingAlias = "aaa";
        var request = new CreateLinkCommand(_newUrlAddress, existingAlias);

        var mockLinkService = new Mock<ILinkService>();
        mockLinkService.Setup(s => s.AliasIsBusy(It.IsAny<string>())).ReturnsAsync(true);

        using var context = DbContextHepler.CreateContext();
        var handler = new CreateLinkCommandHandler(context, mockLinkService.Object);
        int initialLinksCount = await context.Links.CountAsync();

        //act
        Result<LinkCreatedResponse> result = await handler.Handle(request, CancellationToken.None);

        //assert
        Assert.False(result.IsSuccess);
        Assert.False(result.HasValue);
        Assert.NotEmpty(result.Errors!);
        Assert.True(initialLinksCount == await context.Links.CountAsync());
        Assert.True(result.Errors?.Count() == 1);
        Assert.Equal(LinkValidationErrorMessage.ALIAS_TAKEN, result.Errors?.First());
    }


}

