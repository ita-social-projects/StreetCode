using FluentResults;
using MediatR;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Fact.Delete;

public class DeleteFactHandler : IRequestHandler<DeleteFactCommand, Result<Unit>>
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ILoggerService _logger;

    public DeleteFactHandler(IRepositoryWrapper repositoryWrapper, ILoggerService logger)
    {
        _repositoryWrapper = repositoryWrapper;
        _logger = logger;
    }

    public async Task<Result<Unit>> Handle(DeleteFactCommand request, CancellationToken cancellationToken)
    {
        var fact = await _repositoryWrapper.FactRepository.GetFirstOrDefaultAsync(f => f.Id == request.Id);

        if (fact is null)
        {
            string errorMsg = $"Cannot find a fact with corresponding categoryId: {request.Id}";
            _logger.LogError(request, errorMsg);
            return Result.Fail(new Error(errorMsg));
        }

        _repositoryWrapper.FactRepository.Delete(fact);

        var resultIsSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;
        if (resultIsSuccess)
        {
            return Result.Ok(Unit.Value);
        }
        else
        {
            string errorMsg = "Failed to delete a fact";
            _logger.LogError(request, errorMsg);
            return Result.Fail(new Error(errorMsg));
        }
    }
}