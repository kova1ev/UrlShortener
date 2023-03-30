﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Api.Utility;
using UrlShortener.Application.Common.Models;
using UrlShortener.Application.Common.Models.Links;
using UrlShortener.Application.Common.Result;
using UrlShortener.Application.Links.Queries.GetLinkByShortName;
using UrlShortener.Application.Statistic.Commands;

namespace UrlShortener.Api.Controllers
{
    [Route("/r")]
    public class RedirectController : ApiControllerBase<RedirectController>
    {
        public RedirectController(ILogger<RedirectController> logger, IMediator mediator) : base(logger, mediator)
        {

        }

        [HttpGet("{alias}")]
        public async Task<IActionResult> RedirectTo([FromRoute] string alias)
        {
            Result<LinkDto> result = await _mediator.Send(new GetLinkByShortNameQuery(alias));
            if (result.IsSuccess == false)
            {
                return RedirectToPage("/NotFound");
            }

            var agent = HttpContext.Request.Headers["user-agent"];
            UserAgentHelper agentHelper = new();
            UserAgentInfo userAgentInfo = agentHelper.Parse(agent);

            await _mediator.Send(new UpdateLinkStatisticCommand(result.Value.LinkStatistic.Id, userAgentInfo));

            return Redirect(result.Value.UrlAddress);
        }
    }
}

