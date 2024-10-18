using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Timeline;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.News;
using Streetcode.DAL.Entities.Timeline;
using Streetcode.DAL.Helpers;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Timeline.HistoricalContext.GetAll
{
    public class GetAllHistoricalContextHandler : IRequestHandler<GetAllHistoricalContextQuery, Result<GetAllHistoricalContextDTO>>
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

        public Task<Result<GetAllHistoricalContextDTO>> Handle(GetAllHistoricalContextQuery request, CancellationToken cancellationToken)
        {
            PaginationResponse<DAL.Entities.Timeline.HistoricalContext> paginationResponse = _repositoryWrapper
                .HistoricalContextRepository
                .GetAllPaginated(
                    request.page,
                    request.pageSize,
                    descendingSortKeySelector: context => context.Title!);

            if (paginationResponse is null)
            {
                string errorMsg = _stringLocalizerCannotFind["CannotFindAnyHistoricalContexts"].Value;
                _logger.LogError(request, errorMsg);
                return Task.FromResult(Result.Fail<GetAllHistoricalContextDTO>(new Error(errorMsg)));
            }

            GetAllHistoricalContextDTO getAllHistoricalContextDTO = new GetAllHistoricalContextDTO
            {
                TotalAmount = paginationResponse.TotalItems,
                HistoricalContexts = _mapper.Map<IEnumerable<HistoricalContextDTO>>(paginationResponse.Entities)
            };

            return Task.FromResult(Result.Ok(getAllHistoricalContextDTO));
        }
    }
}
