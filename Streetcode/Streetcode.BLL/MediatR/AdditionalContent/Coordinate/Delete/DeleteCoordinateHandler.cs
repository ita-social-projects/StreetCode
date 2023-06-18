using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System.Xml.Linq;

namespace Streetcode.BLL.MediatR.AdditionalContent.Coordinate.Delete;

public class DeleteCoordinateHandler : IRequestHandler<DeleteCoordinateCommand, Result<Unit>>
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IStringLocalizer? _stringLocalizer;

    public DeleteCoordinateHandler(IRepositoryWrapper repositoryWrapper, IStringLocalizer<DeleteCoordinateHandler> stringLocalizer)
    {
        _repositoryWrapper = repositoryWrapper;
        _stringLocalizer = stringLocalizer;
    }

    public async Task<Result<Unit>> Handle(DeleteCoordinateCommand request, CancellationToken cancellationToken)
    {
        var streetcodeCoordinate = await _repositoryWrapper.StreetcodeCoordinateRepository.GetFirstOrDefaultAsync(f => f.Id == request.Id);

        if (streetcodeCoordinate is null)
        {
            return Result.Fail(new Error(_stringLocalizer?["CannotFindCoordinate", request.Id].Value));
        }

        _repositoryWrapper.StreetcodeCoordinateRepository.Delete(streetcodeCoordinate);

        var resultIsSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;
        return resultIsSuccess ? Result.Ok(Unit.Value) : Result.Fail(new Error(_stringLocalizer?["FailedToDeleteCoordinate"].Value));
    }
}