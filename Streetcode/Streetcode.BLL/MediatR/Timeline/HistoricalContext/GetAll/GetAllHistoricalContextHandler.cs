using System.Linq.Expressions;
using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Timeline;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Services.EntityAccessManager;
using Streetcode.BLL.SharedResource;
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

        public GetAllHistoricalContextHandler(
            IRepositoryWrapper repositoryWrapper,
            IMapper mapper,
            ILoggerService logger,
            IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _logger = logger;
            _stringLocalizerCannotFind = stringLocalizerCannotFind;
        }

        public Task<Result<GetAllHistoricalContextDTO>> Handle(
    GetAllHistoricalContextQuery request,
    CancellationToken cancellationToken)
        {
            Expression<Func<DAL.Entities.Timeline.HistoricalContext, bool>>? basePredicate = null;
            var predicate = basePredicate.ExtendWithAccessPredicate(new StreetcodeAccessManager(), request.UserRole, hc => hc.HistoricalContextTimelines, hctl => hctl.Timeline.Streetcode);

            var allContext = _repositoryWrapper
                .HistoricalContextRepository
                .FindAll(predicate)
                .ToList();

            var filteredContext = string.IsNullOrWhiteSpace(request.title)
                ? allContext
                : allContext.Where(t => t.Title.ToLower().Contains(request.title.ToLower())).ToList();

            var totalCount = filteredContext.Count;

            var page = request.page ?? 1;
            var pageSize = request.pageSize ?? 10;

            var paginatedContext = filteredContext
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var resultDto = new GetAllHistoricalContextDTO
            {
                TotalAmount = totalCount,
                HistoricalContexts = _mapper.Map<IEnumerable<HistoricalContextDTO>>(paginatedContext)
            };

            return Task.FromResult(Result.Ok(resultDto));
        }
    }
}