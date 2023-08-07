using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Application.Users.Commands.CreateUser;
using UrlShortener.Api.Attributes;
using UrlShortener.Api.Models;
using UrlShortener.Application.Users.Commands.DeleteUser;
using UrlShortener.Application.Users.Queries.GetAllUsers;
using UrlShortener.Application.Users.Queries.GetById;

namespace UrlShortener.Api.Controllers;

[ApiKeyAuthorize]
[ApiController]
[Route("api/user")]
public class UserController : ApiControllerBase
{
    public UserController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet("{id:guid}", Name = nameof(GetUserById))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserById([FromRoute] Guid id)
    {
        var cancellationToken = HttpContext.RequestAborted;
        var result = await Mediator.Send(new GetUserByIdQuery(id), cancellationToken);
        if (result.IsSuccess == false)
        {
            return NotFound();
        }

        return Ok(result.Value);
    }

    [HttpGet(Name = nameof(GetUsers))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUsers()
    {
        var cancellationToken = HttpContext.RequestAborted;
        var result = await Mediator.Send(new GetAllUsersQuery(), cancellationToken);
        return Ok(result.Value);
    }

    // Commands
    [HttpPost(Name = nameof(CreateUser))]
    [ProducesResponseType(typeof(ApiErrors), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand createUserCommand)
    {
        var cancellationToken = HttpContext.RequestAborted;
        var result = await Mediator.Send(createUserCommand, cancellationToken);
        if (result.IsSuccess == false)
        {
            return BadRequest(ApiErrors.ToBadRequest(result));
        }

        return Ok();
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateUser()
    {
        throw new NotImplementedException();
    }

    [HttpDelete("{id:guid}", Name = nameof(DeleteUser))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiErrors), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteUser([FromRoute] Guid id)
    {
        var cancellationToken = HttpContext.RequestAborted;
        var result = await Mediator.Send(new DeleteUserCommand(id), cancellationToken);
        if (result.IsSuccess == false)
        {
            return BadRequest(ApiErrors.ToBadRequest(result));
        }

        return NoContent();
    }
}