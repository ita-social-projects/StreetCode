using System.Xml.Linq;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.AdditionalContent.Coordinate.Delete;

public class DeleteCoordinateHandler : IRequestHandler<DeleteCoordinateCommand, Result<Unit>>
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFind;
    private readonly IStringLocalizer<FailedToDeleteSharedResource> _stringLocalizerFailedToDelete;

    public DeleteCoordinateHandler(IRepositoryWrapper repositoryWrapper, IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind)
    {
        _repositoryWrapper = repositoryWrapper;
        _stringLocalizerCannotFind = stringLocalizerCannotFind;
    }

    public async Task<Result<Unit>> Handle(DeleteCoordinateCommand request, CancellationToken cancellationToken)
    {
        var streetcodeCoordinate = await _repositoryWrapper.StreetcodeCoordinateRepository.GetFirstOrDefaultAsync(f => f.Id == request.Id);

        if (streetcodeCoordinate is null)
        {
            return Result.Fail(new Error(_stringLocalizerCannotFind?["CannotFindCoordinate", request.Id].Value));
        }

        _repositoryWrapper.StreetcodeCoordinateRepository.Delete(streetcodeCoordinate);

        var resultIsSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;
        return resultIsSuccess ? Result.Ok(Unit.Value) : Result.Fail(new Error(_stringLocalizerCannotFind?["FailedToDeleteCoordinate"].Value));
    }
}