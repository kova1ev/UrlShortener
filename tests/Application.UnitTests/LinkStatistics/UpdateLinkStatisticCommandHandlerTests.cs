using Application.UnitTests.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using UrlShortener.Application.Common.Constants;
using UrlShortener.Application.Common.Domain;
using UrlShortener.Application.Common.Exceptions;
using UrlShortener.Application.Interfaces;
using UrlShortener.Application.LinkStatistics.Commands;
using UrlShortener.Entity;
using Geolocation = UrlShortener.Application.Common.Domain.Geolocation;

namespace Application.UnitTests.LinkStatistics;

public class UpdateLinkStatisticCommandHandlerTests
{
    private readonly string _api = "some-api";
    private readonly DateTime _dateTime = new(2055, 1, 1, 12, 0, 0);

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
    private readonly Mock<IGeolocationService> _mockGeolocationService;

    private readonly Mock<ILogger<UpdateLinkStatisticCommandHandler>> _mockLogger;
    private readonly Mock<IServiceScopeFactory> _mockServiceScopeFactory;

    // TODO 
    public UpdateLinkStatisticCommandHandlerTests()
    {
        _mockSystemDateTime = new Mock<ISystemDateTime>();
        _mockSystemDateTime.Setup(st => st.UtcNow).Returns(_dateTime);

        _mockGeolocationService = new Mock<IGeolocationService>();
        _mockGeolocationService.Setup(gs => gs.GetGeolocationDataAsync(It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync(_geolocation);

        _mockLogger = new Mock<ILogger<UpdateLinkStatisticCommandHandler>>();
        _mockServiceScopeFactory = new Mock<IServiceScopeFactory>();
    }

    [Fact]
    public async Task Update_Should_return_taskCompleted()
    {
        // arrange
        var requestCommand = new UpdateLinkStatisticCommand(_seedLinkLinkStatistic.Id, _userAgentInfo, _api);

        var handler = new UpdateLinkStatisticCommandHandler(_mockSystemDateTime.Object, _mockGeolocationService.Object,
            _mockLogger.Object, _mockServiceScopeFactory.Object);

        // act
        var result = handler.Handle(requestCommand, CancellationToken.None);

        // assert
        Assert.True(result.IsCompleted);
        Assert.True(result.IsCompletedSuccessfully);
        _mockGeolocationService.Verify(gs => gs.GetGeolocationDataAsync(It.IsAny<string>(), CancellationToken.None),
            Times.Once);

        _mockLogger.Verify(l =>
                l.Log(LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, _) => v.ToString()!.Contains("Error")),
                    It.IsAny<ObjectNotFoundException>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
            Times.Never);

        _mockLogger.Verify(l =>
                l.Log(LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, _) => v.ToString()!.Contains("Error")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
            Times.Never);
    }

    [Fact]
    public async Task Update_Should_return_taskCompleted_When_ClientIpIsNUll()
    {
        string? ip = null;
        // arrange
        var requestCommand = new UpdateLinkStatisticCommand(_seedLinkLinkStatistic.Id, _userAgentInfo, ip);

        var handler = new UpdateLinkStatisticCommandHandler(_mockSystemDateTime.Object, _mockGeolocationService.Object,
            _mockLogger.Object, _mockServiceScopeFactory.Object);

        // act
        var result = handler.Handle(requestCommand, CancellationToken.None);

        // assert
        Assert.True(result.IsCompleted);
        Assert.True(result.IsCompletedSuccessfully);
        _mockGeolocationService.Verify(gs => gs.GetGeolocationDataAsync(It.IsAny<string>(), CancellationToken.None),
            Times.Never);
    }
}