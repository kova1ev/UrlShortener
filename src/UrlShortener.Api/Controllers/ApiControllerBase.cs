using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace UrlShortener.Api.Controllers;

public abstract class ApiControllerBase : ControllerBase
{
    private readonly IMediator _mediator;

    protected ApiControllerBase(IMediator mediator)
    {
        _mediator = mediator;
    }

    protected IMediator Mediator => _mediator ?? HttpContext.RequestServices.GetService<IMediator>()!;
}