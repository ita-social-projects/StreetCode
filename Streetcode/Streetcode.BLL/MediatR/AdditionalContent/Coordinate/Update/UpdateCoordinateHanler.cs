using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.AdditionalContent.Coordinate.Update;

public class UpdateCoordinateHandler : IRequestHandler<UpdateCoordinateCommand, Result<Unit>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IStringLocalizer<CannotConvertNullSharedResource> _stringLocalizerCannotConvert;
    private readonly IStringLocalizer<FailedToCreateSharedResource> _stringLocalizerFailedToCreate;

    public UpdateCoordinateHandler(
        IRepositoryWrapper repositoryWrapper,
        IMapper mapper,
        IStringLocalizer<CannotConvertNullSharedResource> stringLocalizerCannotConvert,
        IStringLocalizer<FailedToCreateSharedResource> stringLocalizerFailedToCreate)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _stringLocalizerCannotConvert = stringLocalizerCannotConvert;
        _stringLocalizerFailedToCreate = stringLocalizerFailedToCreate;
    }

    public async Task<Result<Unit>> Handle(UpdateCoordinateCommand request, CancellationToken cancellationToken)
    {
        var streetcodeCoordinate = _mapper.Map<DAL.Entities.AdditionalContent.Coordinates.Types.StreetcodeCoordinate>(request.StreetcodeCoordinate);

        if (streetcodeCoordinate is null)
        {
            return Result.Fail(new Error(_stringLocalizerCannotConvert?["CannotConvertNullToStreetcodeCoordinate"].Value));
        }

        _repositoryWrapper.StreetcodeCoordinateRepository.Update(streetcodeCoordinate);

        var resultIsSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;
        return resultIsSuccess ? Result.Ok(Unit.Value) : Result.Fail(new Error(_stringLocalizerFailedToCreate?["FailedToCreateStreetcodeCoordinate"].Value));
    }
}