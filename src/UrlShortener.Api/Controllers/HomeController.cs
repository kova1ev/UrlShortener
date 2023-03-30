using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace UrlShortener.Api.Controllers
{
    [Route("api/home")]
    public class HomeController : ApiControllerBase<HomeController>
    {

        public HomeController(ILogger<HomeController> logger, IMediator mediator)
            : base(logger, mediator)
        { }


        [HttpGet("headers")]
        public ActionResult TestHeaders()
        {
            var CF_Connecting_IP = HttpContext.Request.Headers["CF-Connecting-IP"];
            var headers = HttpContext.Request.Headers;
            var agent = HttpContext.Request.Headers["user-agent"];

            var a = HttpContext.Connection.LocalIpAddress.ToString();
            var p = HttpContext.Connection.LocalPort.ToString();


            return Ok(agent);
        }

        [HttpGet("geo")]
        public async Task<ActionResult> GetGeo()
        {
            string ipAddress = "89.189.103.132";
            string result = string.Empty;
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://ip-api.com/json/");

                HttpResponseMessage httpResponse = await client.GetAsync(ipAddress);
                httpResponse.EnsureSuccessStatusCode();

                result = await httpResponse.Content.ReadAsStringAsync();

            }


            return Ok(result);
        }
    }
}
