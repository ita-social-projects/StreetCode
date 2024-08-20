using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Timeline;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Timeline.HistoricalContext.GetAll
{
    public class GetAllHistoricalContextHandler : IRequestHandler<GetAllHistoricalContextQuery, Result<IEnumerable<HistoricalContextDTO>>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;
        private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFind;

        public GetAllHistoricalContextHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger, IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _logger = logger;
            _stringLocalizerCannotFind = stringLocalizerCannotFind;
        }

        public async Task<Result<IEnumerable<HistoricalContextDTO>>> Handle(GetAllHistoricalContextQuery request, CancellationToken cancellationToken)
        {
            var historicalContextItems = await _repositoryWrapper
                .HistoricalContextRepository
                .GetAllAsync();

            if (!historicalContextItems.Any())
            {
                string errorMsg = _stringLocalizerCannotFind["CannotFindAnyHistoricalContexts"].Value;
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            var historicalContexts = historicalContextItems.OrderBy(context => context.Title);

            return Result.Ok(_mapper.Map<IEnumerable<HistoricalContextDTO>>(historicalContexts));
        }
    }
}
