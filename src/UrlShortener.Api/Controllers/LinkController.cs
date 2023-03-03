using MediatR;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Application.Commands.Links;
using UrlShortener.Application.Common.Result;
using UrlShortener.Application.Links.Commands.CreateLink;
using UrlShortener.Application.Links.Commands.DeleteLink;
using UrlShortener.Application.Links.Commands.UpdateLink;
using UrlShortener.Application.Links.Queries;
using UrlShortener.Application.Links.Queries.GetLinkByShortName;
using UrlShortener.Application.Links.Queries.GetLinks;
using UrlShortener.Domain.Entity;

namespace UrlShortener.Api.Controllers
{
    [Route("api/link")]
    public class LinkController : ApiControllerBase<LinkController>
    {
        public LinkController(ILogger<LinkController> logger, IMediator mediator) : base(logger, mediator)
        {

        }

        [HttpGet("{shortName}")]
        [ProducesResponseType(typeof(Link), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetByShortName([FromRoute] string shortName)
        {
            LinkDto link = await _mediator.Send(new GetLinkByShortNameQuery(shortName));
            if (link == null)
                return NotFound();
            return Ok(link);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(Link), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetById([FromRoute] Guid id)
        {
            //todo 

            LinkDto link = await _mediator.Send(new GetLinkByIdQuery(id));
            return Ok(link);
        }

        [HttpGet()]
        [ProducesResponseType(typeof(IEnumerable<Link>), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetLinks()
        {
            var links = await _mediator.Send(new GetLinksQuery());
            return Ok(links);
        }

        //  COMMANDS
        [HttpPost]
        [ProducesResponseType(typeof(LinkResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrors), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreteLink([FromBody] CreateLinkDto createLinkDto)
        {
            Result<LinkResponse> result = await _mediator.Send(new CreateLinkCommand(createLinkDto));

            if (result.IsSuccess == false)
            {
                return BadRequest(ApiErrors.ToBadRequest(result));
            }

            return Ok(result.Value);
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrors), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> DeleteLink([FromRoute] Guid id)
        {
            Result result = await _mediator.Send(new DeleteLinkCommand(id));

            if (result.IsSuccess == false)
            {
                return BadRequest(ApiErrors.ToBadRequest(result));
            }

            return Ok(result);
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrors), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateLink([FromRoute] Guid id, [FromBody] UpdateLinkDto updateLinkDto)
        {
            Result result = await _mediator.Send(new UpdateLinkCommand(id, updateLinkDto));
            if (result.IsSuccess == false)
            {
                return BadRequest(ApiErrors.ToBadRequest(result));
            }
            return Ok(result);
        }


        // TEST

        [HttpGet("redirect/{shortName}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> RedirectByLink([FromRoute] string shortName)
        {
            LinkDto link = await _mediator.Send(new GetLinkByShortNameQuery(shortName));
            if (link == null) return NotFound();
            return Redirect(link.UrlAddress);
        }

        [HttpGet("headers")]
        public ActionResult TestHeaders()
        {
            var s = HttpContext.Request.Headers["user-agent"];

            var headers = HttpContext.Request.Headers;
            return Ok(headers);
        }

    }
}
