using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.AdditionalContent.Subtitles;
using Streetcode.BLL.DTO.Timeline;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Timeline.HistoricalContext.GetAll
{
    public class GetAllHistoricalContextHandler : IRequestHandler<GetAllHistoricalContextQuery, Result<IEnumerable<HistoricalContextDTO>>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;

        public GetAllHistoricalContextHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<IEnumerable<HistoricalContextDTO>>> Handle(GetAllHistoricalContextQuery request, CancellationToken cancellationToken)
        {
            var historicalContextItems = await _repositoryWrapper
                .HistoricalContextRepository
                .GetAllAsync();

            if (historicalContextItems is null)
            {
                const string errorMsg = $"Cannot find any historical contexts";
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            return Result.Ok(_mapper.Map<IEnumerable<HistoricalContextDTO>>(historicalContextItems));
        }
    }
}
