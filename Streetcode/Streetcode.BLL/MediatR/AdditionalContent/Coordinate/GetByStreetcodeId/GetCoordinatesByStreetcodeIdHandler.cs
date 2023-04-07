using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.AdditionalContent.Coordinates.Types;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.AdditionalContent.Coordinate.GetByStreetcodeId;

public class GetCoordinatesByStreetcodeIdHandler : IRequestHandler<GetCoordinatesByStreetcodeIdQuery, Result<IEnumerable<StreetcodeCoordinateDTO>>>
{
    private readonly ILoggerService _loggerService;
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public GetCoordinatesByStreetcodeIdHandler(
        IRepositoryWrapper repositoryWrapper,
        IMapper mapper,
        ILoggerService loggerService)
    {
        _loggerService = loggerService;
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<StreetcodeCoordinateDTO>>> Handle(GetCoordinatesByStreetcodeIdQuery request, CancellationToken cancellationToken)
    {
        _loggerService.LogInformation("Entry into GetCoordinatesByStreetcodeIdHandler");

        var coordinates = await _repositoryWrapper.StreetcodeCoordinateRepository
            .GetAllAsync(c => c.StreetcodeId == request.StreetcodeId);

        if (coordinates is null)
        {
            _loggerService.LogError($"Cannot find a coordinates by a streetcode id: {request.StreetcodeId}");
            return Result.Fail(new Error($"Cannot find a coordinates by a streetcode id: {request.StreetcodeId}"));
        }

        var coordinatesDto = _mapper.Map<IEnumerable<StreetcodeCoordinateDTO>>(coordinates);
        return Result.Ok(coordinatesDto);
    }
}
