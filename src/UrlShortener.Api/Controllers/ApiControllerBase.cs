using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace UrlShortener.Api.Controllers
{
    [ApiController]
    public abstract class ApiControllerBase<TApiController> : ControllerBase
    {
        protected readonly ILogger<TApiController> _logger;

        protected readonly IMediator _mediator;

        protected ApiControllerBase(ILogger<TApiController> logger, IMediator mediator)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
    }
}
