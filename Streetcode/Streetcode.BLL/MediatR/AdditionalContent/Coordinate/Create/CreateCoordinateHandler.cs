using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.AdditionalContent.Coordinate.Create;

public class CreateCoordinateHandler : IRequestHandler<CreateCoordinateCommand, Result<Unit>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IStringLocalizer _stringLocalizer;

    public CreateCoordinateHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, IStringLocalizer<CreateCoordinateHandler> stringLocalizer )
    {
        _stringLocalizer = stringLocalizer;
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
    }

    public async Task<Result<Unit>> Handle(CreateCoordinateCommand request, CancellationToken cancellationToken)
    {
        var streetcodeCoordinate = _mapper.Map<DAL.Entities.AdditionalContent.Coordinates.Types.StreetcodeCoordinate>(request.StreetcodeCoordinate);

        if (streetcodeCoordinate is null)
        {
            return Result.Fail(new Error(_stringLocalizer?["CannotConvertNull"].Value));
        }

        _repositoryWrapper.StreetcodeCoordinateRepository.Create(streetcodeCoordinate);

        var resultIsSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;
        return resultIsSuccess ? Result.Ok(Unit.Value) : Result.Fail(new Error(_stringLocalizer?["FailedToCreate"].Value));
    }
}