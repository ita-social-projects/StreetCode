using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Text.Delete;

public class DeleteTextHandler : IRequestHandler<DeleteTextCommand, Result<Unit>>
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ILoggerService _logger;
    private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFind;
    private readonly IStringLocalizer<FailedToDeleteSharedResource> _stringLocalizerFailedToDelete;

    public DeleteTextHandler(
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

    public async Task<Result<Unit>> Handle(DeleteTextCommand request, CancellationToken cancellationToken)
    {
        var text = await _repositoryWrapper.TextRepository.GetFirstOrDefaultAsync(x => x.Id == request.Id);

        if (text is null)
        {
            var errorMessage = _stringLocalizerCannotFind["CannotFindTextWithCorrespondingCategoryId", request.Id].Value;
            _logger.LogError(request, errorMessage);
            return Result.Fail(new Error(errorMessage));
        }

        _repositoryWrapper.TextRepository.Delete(text);
        var resultIsSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;

        if (!resultIsSuccess)
        {
            var errorMessage = _stringLocalizerFailedToDelete["FailedToDeleteText"].Value;
            _logger.LogError(request, errorMessage);
            return Result.Fail(new Error(errorMessage));
        }

        return Result.Ok(Unit.Value);
    }
}
