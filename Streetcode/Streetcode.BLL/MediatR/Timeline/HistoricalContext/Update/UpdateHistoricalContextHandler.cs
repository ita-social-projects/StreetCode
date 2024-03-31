using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Timeline;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Entities.Timeline;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Timeline.HistoricalContext.Update
{
    public class
        UpdateHistoricalContextHandler : IRequestHandler<UpdateHistoricalContextCommand, Result<HistoricalContextDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;

        public UpdateHistoricalContextHandler(IRepositoryWrapper repository, IMapper mapper, ILoggerService logger)
        {
            _repositoryWrapper = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<HistoricalContextDTO>> Handle(
            UpdateHistoricalContextCommand request,
            CancellationToken cancellationToken)
        {
            /* have some shitty code right here, if you delete the DAL.Entities.Timeline. part it will show error
            even if you have using Streetcode.DAL.Entities.Timeline; so sad :( */
            var historicalContext =
                await _repositoryWrapper.HistoricalContextRepository.GetFirstOrDefaultAsync(x =>
                    x.Id == request.HistoricalContext.Id);
            if (historicalContext is null)
            {
                string exMessage = $"No context found by entered Id - {request.HistoricalContext.Id}";
                _logger.LogError(request, exMessage);
                return Result.Fail(exMessage);
            }

            try
            {
                var contextToUpdate = _mapper.Map<DAL.Entities.Timeline.HistoricalContext>(request.HistoricalContext);
                _repositoryWrapper.HistoricalContextRepository.Update(contextToUpdate);
                await _repositoryWrapper.SaveChangesAsync();
                return Result.Ok(_mapper.Map<HistoricalContextDTO>(contextToUpdate));
            }
            catch (Exception ex)
            {
                _logger.LogError(request, ex.Message);
                return Result.Fail(ex.Message);
            }
        }
    }
}