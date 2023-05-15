using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.MediatR.Team.GetAll;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Team.Position.GetAll
{
    public class GetAllPositionsHandler : IRequestHandler<GetAllPositionsQuery, Result<IEnumerable<PositionDTO>>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;

        public GetAllPositionsHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<PositionDTO>>> Handle(GetAllPositionsQuery request, CancellationToken cancellationToken)
        {
            var positions = await _repositoryWrapper
                .PositionRepository
                .GetAllAsync();

            if (positions is null)
            {
                return Result.Fail(new Error($"Cannot find any positions"));
            }

            var positionsDtos = _mapper.Map<IEnumerable<PositionDTO>>(positions);
            return Result.Ok(positionsDtos);
        }
    }
}
