using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace UrlShortener.Application.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next();
        }

        ValidationContext<TRequest> context = new ValidationContext<TRequest>(request);
        ValidationResult[] resultList = _validators.Select(v => v.Validate(context)).ToArray();
        IEnumerable<string> errorsMessage = resultList.SelectMany(x => x.Errors).Where(v => v != null)
                                              .Select(e => e.ErrorMessage)
                                              .ToList();
        if (errorsMessage.Any())
        {
            throw new Common.Exceptions.ValidationException(errorsMessage);
        }
        return await next();
    }
}
