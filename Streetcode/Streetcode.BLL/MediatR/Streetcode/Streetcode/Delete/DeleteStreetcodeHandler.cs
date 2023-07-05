using FluentResults;
using Microsoft.EntityFrameworkCore;
using MediatR;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.SharedResource;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.Delete;

public class DeleteStreetcodeHandler : IRequestHandler<DeleteStreetcodeCommand, Result<Unit>>
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFind;
    private readonly IStringLocalizer<FailedToDeleteSharedResource> _stringLocalizerFailedToDelete;

    public DeleteStreetcodeHandler(
        IRepositoryWrapper repositoryWrapper,
        IStringLocalizer<FailedToDeleteSharedResource> stringLocalizerFailedToDelete,
        IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind)
    {
        _repositoryWrapper = repositoryWrapper;
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
            return Result.Fail(new Error(_stringLocalizerCannotFind["CannotFindStreetcodeWithCorrespondingCategoryId", request.Id]));
        }

        _repositoryWrapper.StreetcodeRepository.Delete(streetcode);

        var resultIsSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;

        return resultIsSuccess ? Result.Ok(Unit.Value) : Result.Fail(new Error(_stringLocalizerFailedToDelete["FailedToDeleteStreetcode"].Value));
    }
}