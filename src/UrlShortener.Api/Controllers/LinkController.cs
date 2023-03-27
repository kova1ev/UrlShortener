using MediatR;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Api.Attributes;
using UrlShortener.Api.Models;
using UrlShortener.Application.Common.Models.Links;
using UrlShortener.Application.Common.Result;
using UrlShortener.Application.Links.Commands.CreateLink;
using UrlShortener.Application.Links.Commands.DeleteLink;
using UrlShortener.Application.Links.Commands.UpdateLink;
using UrlShortener.Application.Links.Queries.GetLinkByShortName;
using UrlShortener.Application.Links.Queries.GetLinks;

namespace UrlShortener.Api.Controllers
{
    [ApiKeyAuthorize]
    [Route("api/link")]
    public class LinkController : ApiControllerBase<LinkController>
    {
        public LinkController(ILogger<LinkController> logger, IMediator mediator) : base(logger, mediator)
        {

        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(LinkDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> GetById([FromRoute] Guid id)
        {
            Result<LinkDto> result = await _mediator.Send(new GetLinkByIdQuery(id));
            if (result.IsSuccess == false)
            {
                return NoContent();
            }
            return Ok(result.Value);
        }

        [HttpGet()]
        [ProducesResponseType(typeof(IEnumerable<LinkDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetLinks()
        {
            // TODO:  PAGINATIONS!
            var links = await _mediator.Send(new GetLinksQuery());
            return Ok(links);
        }

        //  COMMANDS
        [HttpPost]
        [ProducesResponseType(typeof(LinkResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrors), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreteLink([FromBody] CreateLinkModel createLinkDto)
        {
            Result<LinkResponse> result = await _mediator.Send(new CreateLinkCommand(createLinkDto));
            if (result.IsSuccess == false)
            {
                return BadRequest(ApiErrors.ToBadRequest(result));
            }
            return Ok(result.Value);
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiErrors), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> DeleteLink([FromRoute] Guid id)
        {
            Result result = await _mediator.Send(new DeleteLinkCommand(id));
            if (result.IsSuccess == false)
            {
                return BadRequest(ApiErrors.ToBadRequest(result));
            }
            return NoContent();
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiErrors), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateLink([FromRoute] Guid id, [FromBody] UpdateLinkModel updateLinkDto)
        {
            Result result = await _mediator.Send(new UpdateLinkCommand(id, updateLinkDto));
            if (result.IsSuccess == false)
            {
                return BadRequest(ApiErrors.ToBadRequest(result));
            }
            return NoContent();
        }
    }
}
