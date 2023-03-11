using MediatR;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Application.Common.Links;
using UrlShortener.Application.Common.Result;
using UrlShortener.Application.Links.Queries.GetLinkByShortName;

namespace UrlShortener.Api.Controllers
{
    [Route("api/home")]
    public class HomeController : ApiControllerBase<HomeController>
    {
        public HomeController(ILogger<HomeController> logger, IMediator mediator) : base(logger, mediator)
        {

        }


        // TEST features
        [HttpGet("redirect/{shortName}")]
        //   [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> RedirectByLink([FromRoute] string shortName)
        {
            Result<LinkDto> result = await _mediator.Send(new GetLinkByShortNameQuery(shortName));
            if (result.IsSuccess == false)
                return NoContent();
            //return Ok(result.Value);

            return Redirect(result.Value.UrlShort);
        }

        [HttpGet("headers")]
        public ActionResult TestHeaders()
        {
            var headers = HttpContext.Request.Headers;

            var agent = HttpContext.Request.Headers["user-agent"];

            return Ok(headers);
        }
    }
}
