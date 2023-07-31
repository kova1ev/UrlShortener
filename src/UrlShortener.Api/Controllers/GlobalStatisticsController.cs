using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Api.Attributes;
using UrlShortener.Application.GlobalStatistics.Queries.GetLinksCountByTime;
using UrlShortener.Application.GlobalStatistics.Queries.GetTotalLinksCount;

namespace UrlShortener.Api.Controllers;

[ApiController]
[Authorize]
[ApiKeyAuthorize]
[Route("api/statistic")]
public class GlobalStatisticsController : ApiControllerBase
{
    public GlobalStatisticsController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet("totalcount",Name = nameof(GetTotalLinkCount))]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTotalLinkCount()
    {
        var cancellationToken = HttpContext.RequestAborted;
        var result = await Mediator.Send(new GetTotalLinksCountQuery(), cancellationToken);

        return Ok(result.Value);
    }

    [HttpGet("weekcount",Name = nameof(GetLastWeekLinkCount))]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetLastWeekLinkCount([FromQuery] DateOnly? start, [FromQuery] DateOnly? end)
    {
        var cancellationToken = HttpContext.RequestAborted;
        var result = await Mediator.Send(new GetLinksCountByTimeQuery(start, end), cancellationToken);

        return Ok(result.Value);
    }
}