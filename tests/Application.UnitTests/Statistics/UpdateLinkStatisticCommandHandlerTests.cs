using Application.UnitTests.Utility;
using Microsoft.EntityFrameworkCore;
using Moq;
using UrlShortener.Application.Common.Constants;
using UrlShortener.Application.Common.Models;
using UrlShortener.Application.Common.Result;
using UrlShortener.Application.Interfaces;
using UrlShortener.Application.Statistic.Commands;
using UrlShortener.Domain.Entity;

namespace Application.UnitTests.Statistics;

public class UpdateLinkStatisticCommandHandlerTests
{
    private readonly Guid _linkStatisticId = new Guid("1F083C27-6F99-46D7-8120-8299BFA579E1");
    private readonly UrlShortener.Application.Common.Models.Geolocation _geolocation = new()
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
    private readonly DateTime _dateTime = new DateTime(2023, 4, 5, 12, 0, 0);
    private Mock<ISystemDateTime> _mockSystemDateTime;
    public UpdateLinkStatisticCommandHandlerTests()
    {
        _mockSystemDateTime = new Mock<ISystemDateTime>();
        _mockSystemDateTime.Setup(st => st.UtcNow).Returns(_dateTime);
    }


    [Fact]
    public async Task Update_link_Statistic_Success()
    {
        var request = new UpdateLinkStatisticCommand(_linkStatisticId, _userAgentInfo, _geolocation);

        using var context = DbContextHepler.CreateContext();


        var handler = new UpdateLinkStatisticCommandHandler(context, _mockSystemDateTime.Object);

        //act 
        Result result = await handler.Handle(request, CancellationToken.None);
        //assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Errors!);

        LinkStatistic? linkStatistic = await context.LinkStatistics
            .Include(ls => ls.Geolocation)
            .FirstOrDefaultAsync(l => l.Id == _linkStatisticId);

        Assert.NotNull(linkStatistic);
        Assert.True(linkStatistic.Clicks == 1);
        Assert.Equal(_geolocation.City, linkStatistic.Geolocation?.City);
        Assert.Equal(_geolocation.Country, linkStatistic.Geolocation?.Country);
        Assert.Equal(_geolocation.Region, linkStatistic.Geolocation?.Region);
        Assert.Equal(_userAgentInfo.Os, linkStatistic.Os);
        Assert.Equal(_userAgentInfo.Browser, linkStatistic.Browser);
        Assert.Equal(_dateTime, linkStatistic.LastUse);
    }


    [Fact]
    public async Task Update_link_Statistic_with_double_update_Success()
    {
        var request = new UpdateLinkStatisticCommand(_linkStatisticId, _userAgentInfo, _geolocation);

        using var context = DbContextHepler.CreateContext();
        var handler = new UpdateLinkStatisticCommandHandler(context, _mockSystemDateTime.Object);

        //act 
        Result result = await handler.Handle(request, CancellationToken.None);
        await handler.Handle(request, CancellationToken.None);
        //assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Errors!);

        LinkStatistic? linkStatistic = await context.LinkStatistics
            .Include(ls => ls.Geolocation)
            .FirstOrDefaultAsync(l => l.Id == _linkStatisticId);

        Assert.NotNull(linkStatistic);
        Assert.True(linkStatistic.Clicks == 2); // assert this!
        Assert.Equal(_geolocation.City, linkStatistic.Geolocation?.City);
        Assert.Equal(_geolocation.Country, linkStatistic.Geolocation?.Country);
        Assert.Equal(_geolocation.Region, linkStatistic.Geolocation?.Region);
        Assert.Equal(_userAgentInfo.Os, linkStatistic.Os);
        Assert.Equal(_userAgentInfo.Browser, linkStatistic.Browser);
        Assert.Equal(_dateTime, linkStatistic.LastUse);
    }

    [Fact]
    public async Task Try_Update_link_Statistic_with_bad_Id_Failure()
    {
        Guid badId = default!;// Guid.NewGuid(); 
        var request = new UpdateLinkStatisticCommand(badId, _userAgentInfo, _geolocation);

        using var context = DbContextHepler.CreateContext();
        var handler = new UpdateLinkStatisticCommandHandler(context, _mockSystemDateTime.Object);

        //act 
        Result result = await handler.Handle(request, CancellationToken.None);

        //assert
        Assert.False(result.IsSuccess);
        Assert.NotEmpty(result.Errors!);
        Assert.True(result.Errors?.Count() == 1);

        Assert.Equal(LinkStatisticsErrorMessage.NOT_FOUND, result.Errors.First());
    }

}
