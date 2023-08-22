using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.AdditionalContent.Coordinate.Create;

public class CreateCoordinateHandler : IRequestHandler<CreateCoordinateCommand, Result<Unit>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public CreateCoordinateHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
    }

    public async Task<Result<Unit>> Handle(CreateCoordinateCommand request, CancellationToken cancellationToken)
    {
        var streetcodeCoordinate = _mapper.Map<DAL.Entities.AdditionalContent.Coordinates.Types.StreetcodeCoordinate>(request.StreetcodeCoordinate);

        if (streetcodeCoordinate is null)
        {
            return Result.Fail(new Error("Cannot convert null to streetcodeCoordinate"));
        }

        _repositoryWrapper.StreetcodeCoordinateRepository.Create(streetcodeCoordinate);

        var resultIsSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;
        return resultIsSuccess ? Result.Ok(Unit.Value) : Result.Fail(new Error("Failed to create a streetcodeCoordinate"));
    }
}