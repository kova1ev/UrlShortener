using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using UrlShortener.Api.Attributes;
using UrlShortener.Api.Models;
using UrlShortener.Application.Common.Dto;
using UrlShortener.Application.Links.Commands.CreateLink;
using UrlShortener.Application.Links.Commands.DeleteLink;
using UrlShortener.Application.Links.Commands.UpdateLink;
using UrlShortener.Application.Links.Queries.GetLinkById;
using UrlShortener.Application.Links.Queries.GetLinks;
using UrlShortener.Application.Links.Queries.GetMostRedirectedLinks;
using UrlShortener.Application.Responses;

namespace UrlShortener.Api.Controllers;

[ApiController]
[ApiKeyAuthorize]
[Authorize]
[Route("api/link")]
public class LinkController : ApiControllerBase
{
    public LinkController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet("{id:guid}", Name = nameof(GetLinkById))]
    [ProducesResponseType(typeof(LinkDetailsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetLinkById([FromRoute] Guid id)
    {
        var cancellationToken = HttpContext.RequestAborted;
        var result = await Mediator.Send(new GetLinkByIdQuery(id), cancellationToken);
        if (result.IsSuccess == false)
            return NotFound();
        return Ok(result.Value);
    }


    [HttpGet(Name = nameof(GetLinks))]
    [ProducesResponseType(typeof(FilteredPagedData<LinkDetailsResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetLinks([FromQuery] LinksRequestParameters requestParameters)
    {
        var cancellationToken = HttpContext.RequestAborted;
        var result = await Mediator.Send(new GetLinksQuery(requestParameters), cancellationToken);
        return Ok(result.Value);
    }

    [HttpGet("popular", Name = nameof(GetMostPopularLinks))]
    [ProducesResponseType(typeof(IEnumerable<LinkCompactResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetMostPopularLinks()
    {
        var cancellationToken = HttpContext.RequestAborted;
        var result = await Mediator.Send(new GetMostRedirectedLinksQuery(), cancellationToken);

        return Ok(result.Value);
    }

    //  COMMANDS
    [AllowAnonymous]
    [HttpPost]
    [ProducesResponseType(typeof(LinkCreatedResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrors), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreteLink([FromBody] CreateLinkCommand createLinkCommand)
    {
        var cancellationToken = HttpContext.RequestAborted;
        var result = await Mediator.Send(createLinkCommand, cancellationToken);
        if (result.IsSuccess == false)
            return BadRequest(ApiErrors.ToBadRequest(result));
        return Ok(result.Value);
    }

    [HttpDelete("{id:guid}", Name = nameof(DeleteLink))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiErrors), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> DeleteLink([FromRoute] Guid id)
    {
        var cancellationToken = HttpContext.RequestAborted;
        var result = await Mediator.Send(new DeleteLinkCommand(id), cancellationToken);
        if (result.IsSuccess == false)
            return BadRequest(ApiErrors.ToBadRequest(result));
        return NoContent();
    }

    [HttpPut(Name = nameof(UpdateLink))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiErrors), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateLink([FromBody] UpdateLinkCommand updateLinkCommand)
    {
        var cancellationToken = HttpContext.RequestAborted;
        var result = await Mediator.Send(updateLinkCommand, cancellationToken);
        if (result.IsSuccess == false)
            return BadRequest(ApiErrors.ToBadRequest(result));
        return NoContent();
    }
}