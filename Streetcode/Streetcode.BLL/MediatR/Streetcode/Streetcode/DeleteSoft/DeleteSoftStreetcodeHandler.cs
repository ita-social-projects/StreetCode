using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.DeleteSoft;

public class DeleteSoftStreetcodeHandler : IRequestHandler<DeleteSoftStreetcodeCommand, Result<Unit>>
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ILoggerService _logger;
    private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFind;
    private readonly IStringLocalizer<FailedToUpdateSharedResource> _stringLocalizerFailedToUpdate;

    public DeleteSoftStreetcodeHandler(
        IRepositoryWrapper repositoryWrapper,
        ILoggerService logger,
        IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind,
        IStringLocalizer<FailedToUpdateSharedResource> stringLocalizerFailedToUpdate)
    {
        _repositoryWrapper = repositoryWrapper;
        _logger = logger;
        _stringLocalizerCannotFind = stringLocalizerCannotFind;
        _stringLocalizerFailedToUpdate = stringLocalizerFailedToUpdate;
    }

    public async Task<Result<Unit>> Handle(DeleteSoftStreetcodeCommand request, CancellationToken cancellationToken)
    {
        var streetcode = await _repositoryWrapper.StreetcodeRepository
            .GetFirstOrDefaultAsync(f => f.Id == request.Id);

        if (streetcode is null)
        {
            var errorMsg = _stringLocalizerCannotFind["CannotFindAnyStreetcodeWithCorrespondingId", request.Id].Value;
            _logger.LogError(request, errorMsg);
            return Result.Fail(new Error(errorMsg));
        }

        streetcode.Status = DAL.Enums.StreetcodeStatus.Deleted;
        streetcode.UpdatedAt = DateTime.Now;

        _repositoryWrapper.StreetcodeRepository.Update(streetcode);

        var resultIsDeleteSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;

        if(resultIsDeleteSuccess)
        {
            return Result.Ok(Unit.Value);
        }

        var finalErrorMsg = _stringLocalizerFailedToUpdate["FailedToChangeStatusOfStreetcodeToDeleted"].Value;
        _logger.LogError(request, finalErrorMsg);
        return Result.Fail(new Error(finalErrorMsg));
    }
}