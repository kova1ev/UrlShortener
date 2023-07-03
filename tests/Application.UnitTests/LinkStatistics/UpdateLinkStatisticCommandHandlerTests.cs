using Application.UnitTests.Utility;
using Microsoft.EntityFrameworkCore;
using Moq;
using UrlShortener.Application.Common.Constants;
using UrlShortener.Application.Common.Domain;
using UrlShortener.Application.Interfaces;
using UrlShortener.Application.LinkStatistics.Commands;
using UrlShortener.Entity;
using Geolocation = UrlShortener.Application.Common.Domain.Geolocation;

namespace Application.UnitTests.LinkStatistics;

public class UpdateLinkStatisticCommandHandlerTests
{
    private readonly DateTime _dateTime = new(2023, 4, 5, 12, 0, 0);

    private readonly Geolocation _geolocation = new()
    {
        City = "Berlin",
        Country = "Germany",
        Region = "Berlin"
    };

    private readonly UserAgentInfo _userAgentInfo = new()
    {
        Browser = "Opera",
        Os = "Linux"
    };

    private readonly LinkStatistic _seedLinkLinkStatistic = SeedData.Links.First().LinkStatistic!;
    private readonly Mock<ISystemDateTime> _mockSystemDateTime;


    public UpdateLinkStatisticCommandHandlerTests()
    {
        _mockSystemDateTime = new Mock<ISystemDateTime>();
        _mockSystemDateTime.Setup(st => st.UtcNow).Returns(_dateTime);
    }

    [Fact]
    public async Task Update_Should_return_SuccessResult()
    {
        //arrange
        var request = new UpdateLinkStatisticCommand(_seedLinkLinkStatistic.Id, _userAgentInfo, _geolocation);

        using var context = DbContextHelper.CreateContext();
        var handler = new UpdateLinkStatisticCommandHandler(context, _mockSystemDateTime.Object);

        //act 
        var result = await handler.Handle(request, CancellationToken.None);

        //assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Errors);

        var linkStatistic = await context.LinkStatistics
            .Include(ls => ls.Geolocation)
            .FirstOrDefaultAsync(l => l.Id == _seedLinkLinkStatistic.Id);

        Assert.NotNull(linkStatistic);
        Assert.True(linkStatistic.Clicks == _seedLinkLinkStatistic.Clicks + 1);
        Assert.Equal(_geolocation.City, linkStatistic.Geolocation?.City);
        Assert.Equal(_geolocation.Country, linkStatistic.Geolocation?.Country);
        Assert.Equal(_geolocation.Region, linkStatistic.Geolocation?.Region);
        Assert.Equal(_userAgentInfo.Os, linkStatistic.Os);
        Assert.Equal(_userAgentInfo.Browser, linkStatistic.Browser);
        Assert.Equal(_dateTime, linkStatistic.LastUse);
    }


    [Fact]
    public async Task Update_Should_return_SuccessResult_When_doubleUpdate()
    {
        var request = new UpdateLinkStatisticCommand(_seedLinkLinkStatistic.Id, _userAgentInfo, _geolocation);

        using var context = DbContextHelper.CreateContext();
        var handler = new UpdateLinkStatisticCommandHandler(context, _mockSystemDateTime.Object);

        //act 
        var result = await handler.Handle(request, CancellationToken.None);
        await handler.Handle(request, CancellationToken.None);

        //assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Errors);

        var linkStatistic = await context.LinkStatistics
            .Include(ls => ls.Geolocation)
            .FirstOrDefaultAsync(l => l.Id == _seedLinkLinkStatistic.Id);

        Assert.NotNull(linkStatistic);
        Assert.True(linkStatistic.Clicks == _seedLinkLinkStatistic.Clicks + 2); // assert this!
        Assert.Equal(_geolocation.City, linkStatistic.Geolocation?.City);
        Assert.Equal(_geolocation.Country, linkStatistic.Geolocation?.Country);
        Assert.Equal(_geolocation.Region, linkStatistic.Geolocation?.Region);
        Assert.Equal(_userAgentInfo.Os, linkStatistic.Os);
        Assert.Equal(_userAgentInfo.Browser, linkStatistic.Browser);
        Assert.Equal(_dateTime, linkStatistic.LastUse);
    }

    [Fact]
    public async Task Update_Should_return_FailureResult_When_IdIsBad()
    {
        //arrange
        Guid badId = default!; //or Guid.NewGuid(); 
        var request = new UpdateLinkStatisticCommand(badId, _userAgentInfo, _geolocation);

        using var context = DbContextHelper.CreateContext();
        var handler = new UpdateLinkStatisticCommandHandler(context, _mockSystemDateTime.Object);

        //act 
        var result = await handler.Handle(request, CancellationToken.None);

        //assert
        Assert.False(result.IsSuccess);
        Assert.NotEmpty(result.Errors);
        Assert.Single(result.Errors);
        Assert.Equal(LinkStatisticsErrorMessage.NotFound, result.Errors.First());
    }
}