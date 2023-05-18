using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Team;
using Streetcode.DAL.Entities.Team;
using Streetcode.DAL.Repositories.Interfaces.Base;

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
            var newPosition = await _repository.PositionRepository.CreateAsync(new Positions()
            {
                Position = request.position.Position
            });

            try
            {
                _repository.SaveChanges();
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.ToString());
            }

            return Result.Ok(_mapper.Map<PositionDTO>(newPosition));
        }
    }
}