using MediatR;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Application.Links.Queries;
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
            LinkDto link = await _mediator.Send(new GetLinkByShortNameQuery(shortName));
            if (link == null)
                return NotFound();
            return Redirect(link.UrlAddress);
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
