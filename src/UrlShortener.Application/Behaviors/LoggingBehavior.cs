using MediatR;
using Microsoft.Extensions.Logging;
using UrlShortener.Application.Common.Result;

namespace UrlShortener.Application.Behaviors;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Start processing a request {@RequestName}  input {@Request}", request.GetType().Name, request.GetType());

        TResponse response = await next();
        if (response.IsSuccess == false)
        {
            _logger.LogWarning("Request {@RequestName} {@Request} have error: {@ResponseErrors}", request.GetType().Name, request, response.Errors);
        }

        _logger.LogInformation("End of request processing {@ResponseName} return {@Response}", request.GetType().Name, response.GetType());

        return response;
    }
}
