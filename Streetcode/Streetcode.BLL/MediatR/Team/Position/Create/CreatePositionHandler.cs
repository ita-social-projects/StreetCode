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
            try
            {
                var context = _mapper.Map<Positions>(request.position);
                var checkIfContextExists = await _repository.PositionRepository.GetFirstOrDefaultAsync(j => j.Position == request.position.Position);

                if (checkIfContextExists is not null)
                {
                    string exceptionMessege = $"Team position with title '{request.position.Position}' is already exists.";
                    _logger.LogError(request, exceptionMessege);
                    return Result.Fail(exceptionMessege);
                }

                var createdContext = await _repository.PositionRepository.CreateAsync(context);
                await _repository.SaveChangesAsync();
                return Result.Ok(_mapper.Map<PositionDTO>(createdContext));
            }
            catch (Exception ex)
            {
                _logger.LogError(request, ex.Message);
                return Result.Fail(ex.Message);
            }
        }
    }
}
