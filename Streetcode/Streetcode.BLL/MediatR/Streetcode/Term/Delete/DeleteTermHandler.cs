using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Term.Delete;

public class DeleteTermHandler : IRequestHandler<DeleteTermCommand, Result<Unit>>
{
    private readonly IRepositoryWrapper _repository;
    private readonly ILoggerService _logger;
    private readonly IStringLocalizer<CannotConvertNullSharedResource> _stringLocalizerCannotConvert;
    private readonly IStringLocalizer<FailedToDeleteSharedResource> _stringLocalizerFailedToDelete;

    public DeleteTermHandler(
        IRepositoryWrapper repository,
        ILoggerService logger,
        IStringLocalizer<FailedToDeleteSharedResource> stringLocalizerFailedToDelete,
        IStringLocalizer<CannotConvertNullSharedResource> stringLocalizerCannotConvert)
    {
        _repository = repository;
        _logger = logger;
        _stringLocalizerFailedToDelete = stringLocalizerFailedToDelete;
        _stringLocalizerCannotConvert = stringLocalizerCannotConvert;
    }

    public async Task<Result<Unit>> Handle(DeleteTermCommand request, CancellationToken cancellationToken)
    {
        var term = await _repository.TermRepository.GetFirstOrDefaultAsync(x => x.Id == request.id);

        if (term is null)
        {
            var errorMessage = _stringLocalizerCannotConvert["CannotConvertNullToTerm"].Value;
            _logger.LogError(request, errorMessage);
            return Result.Fail(new Error(errorMessage));
        }

        _repository.TermRepository.Delete(term);
        var resultIsSuccess = await _repository.SaveChangesAsync() > 0;

        if (!resultIsSuccess)
        {
            var errorMessage = _stringLocalizerFailedToDelete["FailedToDeleteTerm"].Value;
            _logger.LogError(request, errorMessage);
            return Result.Fail(new Error(errorMessage));
        }

        return Result.Ok(Unit.Value);
    }
}
