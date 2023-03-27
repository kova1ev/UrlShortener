using MediatR;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Application.Common.Models.Links;
using UrlShortener.Application.Common.Result;
using UrlShortener.Application.Links.Queries.GetLinkByShortName;

namespace UrlShortener.Api.Controllers
{
    [Route("/r")]
    [ApiController]
    public class RedirectController : ApiControllerBase<RedirectController>
    {

        public RedirectController(ILogger<RedirectController> logger, IMediator mediator) : base(logger, mediator)
        {

        }

        [HttpGet("{alias}")]
        public async Task<IActionResult> RedirectTo([FromRoute] string alias)
        {
            Result<LinkDto> result = await _mediator.Send(new GetLinkByShortNameQuery(alias));
            if (result.IsSuccess == false)
            {
                return RedirectToPage("/NotFound");
            }
            return Redirect(result.Value.UrlAddress);
        }
    }
}

