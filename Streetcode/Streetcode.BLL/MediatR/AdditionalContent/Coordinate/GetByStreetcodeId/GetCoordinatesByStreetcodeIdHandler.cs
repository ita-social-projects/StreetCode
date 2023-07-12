using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.AdditionalContent.Coordinates.Types;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.AdditionalContent.Coordinate.GetByStreetcodeId;

public class GetCoordinatesByStreetcodeIdHandler : IRequestHandler<GetCoordinatesByStreetcodeIdQuery, Result<IEnumerable<StreetcodeCoordinateDTO>>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ILoggerService _logger;

    public GetCoordinatesByStreetcodeIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<IEnumerable<StreetcodeCoordinateDTO>>> Handle(GetCoordinatesByStreetcodeIdQuery request, CancellationToken cancellationToken)
    {
        if ((await _repositoryWrapper.StreetcodeRepository.GetFirstOrDefaultAsync(s => s.Id == request.StreetcodeId)) is null)
        {
            return Result.Fail(
                new Error($"Cannot find a coordinates by a streetcode id: {request.StreetcodeId}, because such streetcode doesn`t exist"));
        }

        var coordinates = await _repositoryWrapper.StreetcodeCoordinateRepository
            .GetAllAsync(c => c.StreetcodeId == request.StreetcodeId);

        if (coordinates is null)
        {
            string errorMsg = $"Cannot find a coordinates by a streetcode id: {request.StreetcodeId}";
            _logger.LogError(request, errorMsg);
            return Result.Fail(new Error(errorMsg));
        }

        return Result.Ok(_mapper.Map<IEnumerable<StreetcodeCoordinateDTO>>(coordinates));
    }
}
