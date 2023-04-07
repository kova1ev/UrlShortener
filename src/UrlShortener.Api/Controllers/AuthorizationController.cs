using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using UrlShortener.Api.Infrastructure;
using UrlShortener.Api.Models;
using UrlShortener.Application.Common.Models;
using UrlShortener.Application.Common.Result;
using UrlShortener.Application.Users.Queries;

namespace UrlShortener.Api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthorizationController : ApiControllerBase
    {
        private readonly TokenProvider _tokenProvider;
        public AuthorizationController(IMediator mediator, TokenProvider jwtTokenProvider)
            : base(mediator)
        {
            _tokenProvider = jwtTokenProvider;
        }


        [HttpPost("token")]
        [ProducesResponseType(typeof(AuthResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiErrors), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Login([FromBody] Login login)
        {
            Result<User> result = await Mediator.Send(new GetValidUserQuery(login.Email, login.Password));
            if (result.IsSuccess == false)
            {
                return BadRequest(ApiErrors.ToBadRequest(result));
            }

            string token = _tokenProvider.CreateToken(result.Value);

            return Ok(new AuthResponse { Token = token });

        }

        [Authorize]
        [HttpGet()]
        public IActionResult Check()
        {
            //todo check valid token
            return Ok(true);
        }
    }
}

