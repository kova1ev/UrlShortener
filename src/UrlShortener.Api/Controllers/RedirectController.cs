using MediatR;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Api.Utility;
using UrlShortener.Application.Interfaces;
using UrlShortener.Application.Links.Queries.GetLinkByShortName;
using UrlShortener.Application.LinkStatistics.Commands;


namespace UrlShortener.Api.Controllers;

[ApiController]
[Route("/r")]
public class RedirectController : ApiControllerBase
{
    private readonly IConfiguration _configuration;

    public RedirectController(IMediator mediator, IConfiguration configuration) : base(mediator)
    {
        _configuration = configuration;
    }

    //https://mysite/r/{alias}
    [HttpGet("{alias}")]
    public async Task<IActionResult> RedirectTo([FromRoute] string alias)
    {
        var cancellationToken = HttpContext.RequestAborted;

        var result = await Mediator.Send(new GetLinkByShortNameQuery(alias), cancellationToken);
        if (result.IsSuccess == false)
            return RedirectToPage("/NotFound");
        // todo check link if has password: show page for input password
        // valid password  and redirect or access denied 


        var clientIp = HttpContext.Request.GetClientIp();
#if DEBUG
        if (clientIp == null)
            clientIp = _configuration.GetValue<string>("ApiAddress");
#endif
        var userAgentInfo = HttpContext.Request.TryGetUserAgentInfo();

        // run update task
        await Mediator.Send(new UpdateLinkStatisticCommand(result.Value.LinkStatistic.Id, userAgentInfo,
            clientIp), cancellationToken);

        return Redirect(result.Value.UrlAddress!);
    }
}