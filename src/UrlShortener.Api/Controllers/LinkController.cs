using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using UrlShortener.Api.Attributes;
using UrlShortener.Api.Models;
using UrlShortener.Application.Common.Models.Links;
using UrlShortener.Application.Common.Result;
using UrlShortener.Application.Links.Commands.CreateLink;
using UrlShortener.Application.Links.Commands.DeleteLink;
using UrlShortener.Application.Links.Commands.UpdateLink;
using UrlShortener.Application.Links.Queries.GetLinkById;
using UrlShortener.Application.Links.Queries.GetLinks;

namespace UrlShortener.Api.Controllers
{
    [ApiKeyAuthorize]
    [Authorize]
    [Route("api/link")]
    public class LinkController : ApiControllerBase
    {
        public LinkController(IMediator mediator) : base(mediator)
        {

        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(LinkDetailsResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<ActionResult> GetById([FromRoute] Guid id)
        {
            Result<LinkDetailsResponse> result = await Mediator.Send(new GetLinkByIdQuery(id));
            if (result.IsSuccess == false)
            {
                return NoContent();
            }
            return Ok(result.Value);
        }

        [HttpGet()]
        [ProducesResponseType(typeof(IEnumerable<LinkDetailsResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetLinks()
        {
            // TODO:  PAGINATIONS!
            var links = await Mediator.Send(new GetLinksQuery());
            return Ok(links);
        }

        //  COMMANDS
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(typeof(LinkCreatedResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiErrors), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreteLink([FromBody] CreateLinkRequest createLinkRequest)
        {
            Result<LinkCreatedResponse> result = await Mediator.Send(
                new CreateLinkCommand(createLinkRequest.UrlAddress, createLinkRequest.Alias));
            if (result.IsSuccess == false)
            {
                return BadRequest(ApiErrors.ToBadRequest(result));
            }
            return Ok(result.Value);
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ApiErrors), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> DeleteLink([FromRoute] Guid id)
        {
            Result result = await Mediator.Send(new DeleteLinkCommand(id));
            if (result.IsSuccess == false)
            {
                return BadRequest(ApiErrors.ToBadRequest(result));
            }
            return NoContent();
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ApiErrors), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateLink([FromRoute] Guid id, [FromBody] UpdateLinkRequest updateLinkRequest)
        {
            Result result = await Mediator.Send(new UpdateLinkCommand(id, updateLinkRequest.UrlAddress, updateLinkRequest.Alias));
            if (result.IsSuccess == false)
            {
                return BadRequest(ApiErrors.ToBadRequest(result));
            }
            return NoContent();
        }
    }
}
