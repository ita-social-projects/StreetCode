using FluentResults;
using FluentValidation;
using MediatR;

namespace Streetcode.BLL.PipelineBehaviour;

public class ValidationBehaviour<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IResultBase
{
    private readonly IValidator<TRequest>? _compositeValidator;

    public ValidationBehaviour(IValidator<TRequest>? compositeValidator = null)
    {
        _compositeValidator = compositeValidator;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (_compositeValidator is null)
        {
            return await next();
        }

        var validationResult = await _compositeValidator.ValidateAsync(request, cancellationToken);

        if (validationResult.IsValid)
        {
            return await next();
        }

        return (dynamic)Result.Fail(validationResult.Errors.Select(e => new Error(e.ErrorMessage)).ToList());
    }
}
