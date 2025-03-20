using System.Linq.Expressions;
using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Timeline;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Services.EntityAccessManager;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Timeline.HistoricalContext.GetById
{
    public class GetHistoricalContextByIdHandler : IRequestHandler<GetHistoricalContextByIdQuery, Result<HistoricalContextDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repository;
        private readonly ILoggerService _loggerService;
        private readonly IStringLocalizer<CannotFindSharedResource> _localizer;

        public GetHistoricalContextByIdHandler(IMapper mapper, IRepositoryWrapper repository, ILoggerService loggerService, IStringLocalizer<CannotFindSharedResource> localizer)
        {
            _mapper = mapper;
            _repository = repository;
            _loggerService = loggerService;
            _localizer = localizer;
        }

        public async Task<Result<HistoricalContextDTO>> Handle(GetHistoricalContextByIdQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<DAL.Entities.Timeline.HistoricalContext, bool>>? basePredicate = hc => hc.Id == request.ContextId;
            var predicate = basePredicate.ExtendWithAccessPredicate(new StreetcodeAccessManager(), request.UserRole, hc => hc.HistoricalContextTimelines, hctl => hctl.Timeline.Streetcode);

            var context = await _repository.HistoricalContextRepository.GetFirstOrDefaultAsync(predicate: predicate);

            if (context is null)
            {
                string exceptionMessege = _localizer["CannotFindHistoricalContextWithCorrespondingId", request.ContextId];
                _loggerService.LogError(request, exceptionMessege);
                return Result.Fail(exceptionMessege);
            }

            try
            {
                var contextDto = _mapper.Map<HistoricalContextDTO>(context);
                return Result.Ok(contextDto);
            }
            catch (Exception ex)
            {
                _loggerService.LogError(request, ex.Message);
                return Result.Fail(ex.Message);
            }
        }
    }
}