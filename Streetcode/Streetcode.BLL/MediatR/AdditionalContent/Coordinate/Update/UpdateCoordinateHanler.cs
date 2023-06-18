using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.AdditionalContent.Coordinate.Update;

public class UpdateCoordinateHandler : IRequestHandler<UpdateCoordinateCommand, Result<Unit>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IStringLocalizer? _stringLocalizer;

    public UpdateCoordinateHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, IStringLocalizer<UpdateCoordinateHandler> stringLocalizer)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _stringLocalizer = stringLocalizer;
    }

    public async Task<Result<Unit>> Handle(UpdateCoordinateCommand request, CancellationToken cancellationToken)
    {
        var streetcodeCoordinate = _mapper.Map<DAL.Entities.AdditionalContent.Coordinates.Types.StreetcodeCoordinate>(request.StreetcodeCoordinate);

        if (streetcodeCoordinate is null)
        {
            return Result.Fail(new Error(_stringLocalizer?["CannotConvertNull"].Value));
        }

        _repositoryWrapper.StreetcodeCoordinateRepository.Update(streetcodeCoordinate);

        var resultIsSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;
        return resultIsSuccess ? Result.Ok(Unit.Value) : Result.Fail(new Error(_stringLocalizer?["FailedToCreate"].Value));
    }
}