using FluentResults;
using MediatR;
using Streetcode.BLL.Interfaces.Logging;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Fact.Delete;

public class DeleteFactHandler : IRequestHandler<DeleteFactCommand, Result<Unit>>
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ILoggerService _logger;
    private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFind;
    private readonly IStringLocalizer<FailedToDeleteSharedResource> _stringLocalizerFailedToDelete;

    public DeleteFactHandler(
        IRepositoryWrapper repositoryWrapper,
        ILoggerService logger,
        IStringLocalizer<FailedToDeleteSharedResource> stringLocalizerFailedToDelete,
        IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind)
    {
        _repositoryWrapper = repositoryWrapper;
        _logger = logger;
        _stringLocalizerFailedToDelete = stringLocalizerFailedToDelete;
        _stringLocalizerCannotFind = stringLocalizerCannotFind;
    }

    public async Task<Result<Unit>> Handle(DeleteFactCommand request, CancellationToken cancellationToken)
    {
        var fact = await _repositoryWrapper.FactRepository.GetFirstOrDefaultAsync(f => f.Id == request.Id);

        if (fact is null)
        {
            string errorMsg = _stringLocalizerCannotFind["CannotFindFactWithCorrespondingCategoryId", request.Id].Value;
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
            string errorMsg = _stringLocalizerFailedToDelete["FailedToDeleteFact"].Value;
            _logger.LogError(request, errorMsg);
            return Result.Fail(new Error(errorMsg));
        }
    }
}