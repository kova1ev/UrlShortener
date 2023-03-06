using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace UrlShortener.Api.Controllers
{
    [Route("api/home")]
    public class HomeController : ApiControllerBase<HomeController>
    {
        public HomeController(ILogger<HomeController> logger, IMediator mediator) : base(logger, mediator)
        {

        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            _logger.LogCritical("is ++ {0} ", typeof(HomeController));
            string name = HttpContext.Request.Host.ToString();
            return Ok(name);
        }
    }
}
