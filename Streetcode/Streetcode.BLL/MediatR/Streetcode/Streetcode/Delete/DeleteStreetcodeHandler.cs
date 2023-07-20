using FluentResults;
using Microsoft.EntityFrameworkCore;
using MediatR;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.BLL.Interfaces.Logging;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.SharedResource;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.Delete;

public class DeleteStreetcodeHandler : IRequestHandler<DeleteStreetcodeCommand, Result<Unit>>
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ILoggerService _logger;
    private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFind;
    private readonly IStringLocalizer<FailedToDeleteSharedResource> _stringLocalizerFailedToDelete;

    public DeleteStreetcodeHandler(
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

    public async Task<Result<Unit>> Handle(DeleteStreetcodeCommand request, CancellationToken cancellationToken)
    {
        var streetcode = await _repositoryWrapper.StreetcodeRepository
            .GetFirstOrDefaultAsync(
            predicate: s => s.Id == request.Id,
            include: s => s.Include(x => x.Observers)
                           .Include(x => x.Targets));

        if (streetcode is null)
        {
            string errorMsg = _stringLocalizerCannotFind["CannotFindStreetcodeWithCorrespondingCategoryId", request.Id].Value;
            _logger.LogError(request, errorMsg);
            return Result.Fail(new Error(errorMsg));
        }

        _repositoryWrapper.StreetcodeRepository.Delete(streetcode);

        var resultIsSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;

        if(resultIsSuccess)
        {
            return Result.Ok(Unit.Value);
        }
        else
        {
            string errorMsg = _stringLocalizerFailedToDelete["FailedToDeleteStreetcode"].Value;
            _logger.LogError(request, errorMsg);
            return Result.Fail(new Error(errorMsg));
        }
    }
}