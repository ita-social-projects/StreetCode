using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.DAL.Entities.AdditionalContent.Coordinates.Types;

namespace Streetcode.BLL.MediatR.AdditionalContent.Coordinate.Create;

public class CreateCoordinateHandler : IRequestHandler<CreateCoordinateCommand, Result<Unit>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IStringLocalizer<CannotConvertNullSharedResource> _stringLocalizer;
    private readonly IStringLocalizer<FailedToCreateSharedResource> _stringLocalizerFaild;

    public CreateCoordinateHandler(
        IRepositoryWrapper repositoryWrapper,
        IMapper mapper,
        IStringLocalizer<CannotConvertNullSharedResource> stringLocalizer,
        IStringLocalizer<FailedToCreateSharedResource> stringLocalizerFaild)
    {
        _stringLocalizer = stringLocalizer;
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _stringLocalizerFaild = stringLocalizerFaild;
    }

    public async Task<Result<Unit>> Handle(CreateCoordinateCommand request, CancellationToken cancellationToken)
    {
        var streetcodeCoordinate = _mapper.Map<StreetcodeCoordinate>(request.StreetcodeCoordinate);

        if (streetcodeCoordinate is null)
        {
            return Result.Fail(new Error(_stringLocalizer?["CannotConvertNullToStreetcodeCoordinate"].Value));
        }

        _repositoryWrapper.StreetcodeCoordinateRepository.Create(streetcodeCoordinate);

        var resultIsSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;
        return resultIsSuccess ? Result.Ok(Unit.Value) : Result.Fail(new Error(_stringLocalizerFaild?["FailedToCreateStreetcodeCoordinate"].Value));
    }
}