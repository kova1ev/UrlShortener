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
        public RedirectController(IMediator mediator, IGeolocationService geolocationService, IConfiguration configuration)
            : base(mediator)
        {
            _geolocationService = geolocationService;
            _configuration = configuration;
        }

        [HttpGet("{alias}")]
        public async Task<IActionResult> RedirectTo([FromRoute] string alias)
        {
            Result<LinkDto> result = await Mediator.Send(new GetLinkByShortNameQuery(alias));
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
            Geolocation data = await _geolocationService.GetData(clientIp);

            string? agent = HttpContext.Request.Headers["user-agent"];
            UserAgentHelper agentHelper = new();
            UserAgentInfo userAgentInfo = agentHelper.Parse(agent);

            await Mediator.Send(new UpdateLinkStatisticCommand(result.Value.LinkStatistic.Id, userAgentInfo, data));

            return Redirect(result.Value.UrlAddress);
        }
    }
}

