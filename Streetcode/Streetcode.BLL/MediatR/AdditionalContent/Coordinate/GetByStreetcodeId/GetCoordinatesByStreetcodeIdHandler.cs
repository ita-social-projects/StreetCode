using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.AdditionalContent.Coordinates.Types;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.AdditionalContent.Coordinate.GetByStreetcodeId
{
    public class GetCoordinatesByStreetcodeIdHandler : IRequestHandler<GetCoordinatesByStreetcodeIdQuery, Result<IEnumerable<StreetcodeCoordinateDTO>>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;

        public GetCoordinatesByStreetcodeIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<StreetcodeCoordinateDTO>>> Handle(GetCoordinatesByStreetcodeIdQuery request, CancellationToken cancellationToken)
        {
            var coordinates = await _repositoryWrapper.StreetcodeCoordinateRepository
                .GetAllAsync(c => c.StreetcodeId == request.streetcodeId);

            if (coordinates is null)
            {
                return Result.Fail(new Error($"Cannot find a coordinates by a streetcode id: {request.streetcodeId}"));
            }

            var coordinatesDto = _mapper.Map<IEnumerable<StreetcodeCoordinateDTO>>(coordinates);
            return Result.Ok(coordinatesDto);
        }
    }
}
