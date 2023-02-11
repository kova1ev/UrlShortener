using Microsoft.AspNetCore.Mvc;

namespace UrlShortener.Api.Controllers
{
    public class HomeController : ApiControllerBase<HomeController>
    {
        public HomeController(ILogger<HomeController> logger) : base(logger)
        {
        }

        [HttpGet]
        public IActionResult Index()
        {
            _logger.LogCritical("is ++ {0} ", typeof(HomeController));
            return Ok("ok");
        }
    }
}
