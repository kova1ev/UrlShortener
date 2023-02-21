using MediatR;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Application.Dto.Link;
using UrlShortener.Application.Links.Commands.CreateLink;
using UrlShortener.Application.Links.Queries.GetLinkByShortName;
using UrlShortener.Application.Links.Queries.GetLinks;
using UrlShortener.Application.Responses;
using UrlShortener.Domain.Entity;

namespace UrlShortener.Api.Controllers
{
    public class LinkController : ApiControllerBase<LinkController>
    {

        public LinkController(ILogger<LinkController> logger, IMediator mediator) : base(logger, mediator)
        {

        }

        [HttpGet("{shortName}", Name = "getLinkByShortName")]
        [ProducesResponseType(typeof(Link), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetByShortName([FromRoute] string shortName)
        {
            Link link = await _mediator.Send(new GetLinkByShortNameQuery(shortName));
            if (link == null)
                return NotFound();
            return Ok(link);
        }

        [HttpGet("{id:guid}", Name = "getLinkByID")]
        [ProducesResponseType(typeof(Link), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetById([FromRoute] Guid id)
        {

            Link link = await _mediator.Send(new GetLinkByIdQuery(id));
            if (link == null)
                return NotFound();
            return Ok(link);
        }


        [HttpGet(Name = "getAll")]
        [ProducesResponseType(typeof(IEnumerable<Link>), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetLinks()
        {
            var links = await _mediator.Send(new GetLinksQuery());
            return Ok(links);
        }

        [HttpPost(Name = "createLink")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreteLink([FromBody] CreateLinkDto creteLinkDto)
        {
            CommandResult<Guid> result = await _mediator.Send(new CreateLinkCommand(creteLinkDto));
            if (result.IsSuccess == false)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok(result.ReturnedObject);
        }




        // TEST

        [HttpGet("redirect/{shortName}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> RedirectByLink([FromRoute] string shortName)
        {
            Link link = await _mediator.Send(new GetLinkByShortNameQuery(shortName));
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
