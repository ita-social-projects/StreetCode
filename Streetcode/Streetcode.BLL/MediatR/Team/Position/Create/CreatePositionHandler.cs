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
        private readonly ILoggerService? _logger;

        public CreatePositionHandler(IMapper mapper, IRepositoryWrapper repository, ILoggerService? logger = null)
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
                _logger?.LogError("CreatePositionQuery handled with an error");
                _logger?.LogError(ex.Message);
                return Result.Fail(ex.Message);
            }

            _logger?.LogInformation($"CreatePositionQuery handled successfully");
            return Result.Ok(_mapper.Map<PositionDTO>(newPosition));
        }
    }
}