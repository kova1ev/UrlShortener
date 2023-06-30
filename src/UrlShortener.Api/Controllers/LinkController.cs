using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using UrlShortener.Api.Models;
using UrlShortener.Application.Common.Domain;
using UrlShortener.Application.Common.Domain.Links;
using UrlShortener.Application.Links.Commands.CreateLink;
using UrlShortener.Application.Links.Commands.DeleteLink;
using UrlShortener.Application.Links.Commands.UpdateLink;
using UrlShortener.Application.Links.Queries.GetLinkById;
using UrlShortener.Application.Links.Queries.GetLinks;
using UrlShortener.Application.Links.Queries.GetMostRedirectedLinks;

namespace UrlShortener.Api.Controllers;

[ApiController]
// [ApiKeyAuthorize]
// [Authorize]
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
        var cancellationToken = HttpContext.RequestAborted;
        var result = await Mediator.Send(new GetLinkByIdQuery(id), cancellationToken);
        if (result.IsSuccess == false)
            return NoContent();
        return Ok(result.Value);
    }


    [HttpGet]
    [ProducesResponseType(typeof(FilteredPagedData<LinkDetailsResponse>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> GetLinks([FromQuery] LinksRequestParameters requestParameters)
    {
        var cancellationToken = HttpContext.RequestAborted;
        var result = await Mediator
            .Send(new GetLinksQuery(requestParameters), cancellationToken);
        return Ok(result.Value);
    }

    [HttpGet("popular")]
    [ProducesResponseType(typeof(IEnumerable<LinkCompactResponse>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> GetMostPopular()
    {
        var cancellationToken = HttpContext.RequestAborted;
        var result = await Mediator.Send(new GetMostRedirectedLinksQuery(),cancellationToken);

        return Ok(result.Value);
    }

    //  COMMANDS
    [AllowAnonymous]
    [HttpPost]
    [ProducesResponseType(typeof(LinkCreatedResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ApiErrors), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> CreteLink([FromBody] CreateLinkRequest createLinkRequest)
    {
        var cancellationToken = HttpContext.RequestAborted;
        var result = await Mediator.Send(new CreateLinkCommand(createLinkRequest.UrlAddress, createLinkRequest.Alias),
            cancellationToken);
        if (result.IsSuccess == false)
            return BadRequest(ApiErrors.ToBadRequest(result));
        return Ok(result.Value);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType(typeof(ApiErrors), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult> DeleteLink([FromRoute] Guid id)
    {
        var cancellationToken = HttpContext.RequestAborted;
        var result = await Mediator.Send(new DeleteLinkCommand(id), cancellationToken);
        if (result.IsSuccess == false)
            return BadRequest(ApiErrors.ToBadRequest(result));
        return NoContent();
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType(typeof(ApiErrors), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> UpdateLink([FromRoute] Guid id, [FromBody] UpdateLinkRequest updateLinkRequest)
    {
        var cancellationToken = HttpContext.RequestAborted;
        var result =
            await Mediator.Send(new UpdateLinkCommand(id, updateLinkRequest.UrlAddress, updateLinkRequest.Alias),
                cancellationToken);
        if (result.IsSuccess == false)
            return BadRequest(ApiErrors.ToBadRequest(result));
        return NoContent();
    }
}