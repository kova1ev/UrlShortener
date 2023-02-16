using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace UrlShortener.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class ApiControllerBase<TController> : ControllerBase
    //where TController : ControllerBase
    {
        protected readonly ILogger<TController> _logger;

        protected readonly IMediator _mediator;

        protected ApiControllerBase(ILogger<TController> logger, IMediator mediator)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
    }
}
