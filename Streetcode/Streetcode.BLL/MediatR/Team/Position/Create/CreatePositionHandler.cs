using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Entities.Team;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Team.Create
{
    public class CreatePositionHandler : IRequestHandler<CreatePositionQuery, Result<PositionDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repository;
        private readonly ILoggerService _logger;

        public CreatePositionHandler(IMapper mapper, IRepositoryWrapper repository, ILoggerService logger)
        {
            _mapper = mapper;
            _repository = repository;
            _logger = logger;
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
                _logger.LogError(request, ex.Message);
                return Result.Fail(ex.Message);
            }

            return Result.Ok(_mapper.Map<PositionDTO>(newPosition));
        }
    }
}