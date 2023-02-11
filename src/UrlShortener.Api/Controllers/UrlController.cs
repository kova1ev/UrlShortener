using Microsoft.AspNetCore.Mvc;
using UrlShortener.Application.Interfaces;
using UrlShortener.Domain.Entity;

namespace UrlShortener.Api.Controllers
{
    public class UrlController : ApiControllerBase<UrlController>
    {
        private readonly IAppDbContext context;
        public UrlController(ILogger<UrlController> logger, IAppDbContext context) : base(logger)
        {
            this.context = context;
        }

        [HttpGet]
        [ProducesResponseType(typeof(Url), StatusCodes.Status200OK)]
        public ActionResult GetUrl()
        {
            _logger.LogCritical("is ** {0} ", typeof(UrlController));
            //Url url = new Url("https://code-maze.com/aspnetcore-web-api-return-types/", "https://goo.su/banana") { Id = Guid.NewGuid() };
            Url? url = context.Urls.FirstOrDefault();
            return Ok(url != null ? url : "NULL");
        }


        [HttpGet("redirect")]
        public ActionResult UrlRediret(string? url)
        {
            // string s = url.ToLower();
            return Redirect("https://goo.su/banana");
        }



        private void Foo()
        {

        }
    }
}
