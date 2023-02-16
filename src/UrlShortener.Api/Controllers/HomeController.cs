using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Application.Interfaces;

namespace UrlShortener.Api.Controllers
{
    public class HomeController : ApiControllerBase<HomeController>
    {

        private readonly IAppDbContext context;
        public HomeController(ILogger<HomeController> logger, IAppDbContext context, IMediator mediator) : base(logger, mediator)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var links = await context.Links.ToArrayAsync();
            _logger.LogCritical("is ++ {0} ", typeof(HomeController));
            return Ok(links);
        }
    }
}
