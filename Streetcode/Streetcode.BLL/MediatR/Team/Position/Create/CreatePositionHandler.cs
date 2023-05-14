using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Team;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.DAL.Repositories.Realizations.Base;

namespace Streetcode.BLL.MediatR.Team.Create
{
    public class CreatePositionHandler : IRequestHandler<CreatePositionQuery, Result<PositionDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repository;

        public CreatePositionHandler(IMapper mapper, IRepositoryWrapper repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<Result<PositionDTO>> Handle(CreatePositionQuery request, CancellationToken cancellationToken)
        {
            var position = _mapper.Map<DAL.Entities.Team.Positions>(request.position);

            if (position is null)
            {
                return Result.Fail(new Error("Cannot convert null to position"));
            }

            var createdPosition = _repository.PositionRepository.Create(position);

            if (createdPosition is null)
            {
                return Result.Fail(new Error("Cannot create position"));
            }

            var resultIsSuccess = await _repository.SaveChangesAsync() > 0;

            if (!resultIsSuccess)
            {
                return Result.Fail(new Error("Failed to create a position"));
            }

            var createdPositionDTO = _mapper.Map<PositionDTO>(createdPosition);

            return createdPositionDTO != null ? Result.Ok(createdPositionDTO) : Result.Fail(new Error("Failed to map created position"));
        }
    }
}
