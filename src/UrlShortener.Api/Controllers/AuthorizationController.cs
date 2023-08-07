using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using UrlShortener.Api.Attributes;
using UrlShortener.Api.Infrastructure;
using UrlShortener.Api.Models;
using UrlShortener.Application.Authorization.Commands.Login;
using UrlShortener.Application.Authorization.Commands.RefreshToken;
using UrlShortener.Application.Users.Commands.CreateUser;
using UrlShortener.Application.Users.Queries;
using YamlDotNet.Core.Tokens;
using Microsoft.AspNetCore.Http;
using UrlShortener.Application.Authorization.Commands.RevokeRefreshToken;

namespace UrlShortener.Api.Controllers;

[ApiController]
[ApiKeyAuthorize]
[Route("api/auth")]
public class AuthorizationController : ApiControllerBase
{
    public AuthorizationController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrors), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login([FromBody] LoginCommand login)
    {
        var cancellationToken = HttpContext.RequestAborted;
        var result = await Mediator.Send(login, cancellationToken);
        if (result.IsSuccess == false)
        {
            return BadRequest(ApiErrors.ToBadRequest(result));
        }

        return Ok(result.Value);
    }


    [HttpPost("refresh-jwttoken")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrors), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshAccessTokenCommand refreshAccessTokenCommand)
    {
        var cancellationToken = HttpContext.RequestAborted;
        var result = await Mediator.Send(refreshAccessTokenCommand, cancellationToken);
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return BadRequest(ApiErrors.ToBadRequest(result));
    }

    [Authorize]
    [HttpPost("revoke/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrors), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Revoke([FromRoute] Guid id)
    {
        var cancellationToken = HttpContext.RequestAborted;
        var result = await Mediator.Send(new RevokeRefreshTokenCommand(id), cancellationToken);
        if (result.IsSuccess == false)
        {
            return BadRequest(ApiErrors.ToBadRequest(result));
        }

        return Ok();
    }
}