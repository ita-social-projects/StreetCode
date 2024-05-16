using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Timeline;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Timeline.HistoricalContext.Create
{
    public class CreateHistoricalContextHandler : IRequestHandler<CreateHistoricalContextCommand, Result<HistoricalContextDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;

        public CreateHistoricalContextHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper, ILoggerService logger)
        {
            _mapper = mapper;
            _repositoryWrapper = repositoryWrapper;
            _logger = logger;
        }

        public async Task<Result<HistoricalContextDTO>> Handle(CreateHistoricalContextCommand request, CancellationToken cancellationToken)
        {
            try
            {
                /* read comment at Streetcode.BLL.MediatR.Timeline.HistoricalContext.Update line 29 */
                var context = _mapper.Map<DAL.Entities.Timeline.HistoricalContext>(request.context);
                var checkIfContextExists = await _repositoryWrapper.HistoricalContextRepository.GetFirstOrDefaultAsync(j => j.Title == request.context.Title);

                if (checkIfContextExists is not null)
                {
                    string exceptionMessege = $"Context with title '{request.context.Title}' is already exists.";
                    _logger.LogError(request, exceptionMessege);
                    return Result.Fail(exceptionMessege);
                }

                var createdContext = await _repositoryWrapper.HistoricalContextRepository.CreateAsync(context);
                await _repositoryWrapper.SaveChangesAsync();
                return Result.Ok(_mapper.Map<HistoricalContextDTO>(createdContext));
            }
            catch (Exception ex)
            {
                _logger.LogError(request, ex.Message);
                return Result.Fail(ex.Message);
            }
        }
    }
}