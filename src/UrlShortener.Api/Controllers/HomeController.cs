using MediatR;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Application.Common.Models.Links;
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
        [HttpGet("{shortName}")]
        public async Task<ActionResult> RedirectByLink([FromRoute] string shortName)
        {
            throw new DivideByZeroException();
            Result<LinkDto> result = await _mediator.Send(new GetLinkByShortNameQuery(shortName));
            // string urlForreedirect = "https://github.com/kova1ev";
            // return Redirect(urlForreedirect);

            if (result.IsSuccess == false)
                return NoContent();
            //return Ok(result.Value);

            return Redirect(result.Value.UrlAddress);
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
