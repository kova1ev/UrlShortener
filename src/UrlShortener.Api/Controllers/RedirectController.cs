using MediatR;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Api.Infrastructure;
using UrlShortener.Api.Utility;
using UrlShortener.Application.Common.Models;
using UrlShortener.Application.Common.Models.Links;
using UrlShortener.Application.Common.Result;
using UrlShortener.Application.Links.Queries.GetLinkByShortName;
using UrlShortener.Application.Statistic.Commands;

namespace UrlShortener.Api.Controllers
{
    [Route("/r")]
    public class RedirectController : ApiControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IGeolocationService _geolocationService;
        private readonly ILogger<RedirectController> _logger;
        public RedirectController(IMediator mediator, IGeolocationService geolocationService,
            IConfiguration configuration, ILogger<RedirectController> logger)
            : base(mediator)
        {
            _geolocationService = geolocationService;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpGet("{alias}")]
        public async Task<IActionResult> RedirectTo([FromRoute] string alias)
        {
            Result<LinkDetailsResponse> result = await Mediator.Send(new GetLinkByShortNameQuery(alias));
            if (result.IsSuccess == false)
            {
                return RedirectToPage("/NotFound");
            }

            ClientIpHelper clientIpHelper = new ClientIpHelper();
            string? clientIp = clientIpHelper.GetClientIpByCloudFlare(HttpContext);

#if DEBUG
            if (clientIp == null)
                clientIp = _configuration.GetValue<string>("ApiAddress");
#endif
            // TODO 1-2 sec server latency => make background work 
            // todo add add canceled 1-2 sec?
            Geolocation geolocation = new();
            if (clientIp != null)
            {
                geolocation = await _geolocationService.GetData(clientIp);
            }

            string? agent = HttpContext.Request.Headers["user-agent"];
            UserAgentHelper agentHelper = new();
            UserAgentInfo userAgentInfo = agentHelper.Parse(agent);

            Result resultUpdate = await Mediator.Send(new UpdateLinkStatisticCommand(result.Value.LinkStatistic.Id, userAgentInfo, geolocation));
            if (result.IsSuccess == false)
                _logger.LogError("Link id: {0} - {1}", result.Value.Id, resultUpdate.Errors?.First());

            return Redirect(result.Value.UrlAddress!);
        }
    }
}

