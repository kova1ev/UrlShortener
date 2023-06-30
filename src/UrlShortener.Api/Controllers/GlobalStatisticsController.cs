using MediatR;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Api.Attributes;
using UrlShortener.Application.GlobalStatistics.Queries.GetLinksCountByTime;
using UrlShortener.Application.GlobalStatistics.Queries.GetTotalLinksCount;
using UrlShortener.Application.Interfaces;

namespace UrlShortener.Api.Controllers;

[ApiController]
[ApiKeyAuthorize]
[Route("api/statistic")]
public class GlobalStatisticsController : ApiControllerBase
{
    public GlobalStatisticsController(IMediator mediator, IAppDbContext dbContext,
        ILogger<GlobalStatisticsController> logger) : base(mediator)
    {
    }

    [HttpGet("totalcount")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTotalLinkCount()
    {
        var cancellationToken = HttpContext.RequestAborted;
        var result = await Mediator.Send(new GetTotalLinksCount(), cancellationToken);

        return Ok(result.Value);
    }

    [HttpGet("weekcount")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetLastWeekLinkCount([FromQuery] DateOnly? start, [FromQuery] DateOnly? end)
    {
        var cancellationToken = HttpContext.RequestAborted;
        var result = await Mediator.Send(new GetLinksCountByTime(start, end), cancellationToken);

        return Ok(result.Value);
    }
}