using System.Linq.Expressions;
using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.DTO.Timeline;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Services.EntityAccessManager;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Timeline.TimelineItem.GetByStreetcodeId;

public class GetTimelineItemsByStreetcodeIdHandler : IRequestHandler<GetTimelineItemsByStreetcodeIdQuery, Result<IEnumerable<TimelineItemDTO>>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ILoggerService _logger;

    public GetTimelineItemsByStreetcodeIdHandler(
        IRepositoryWrapper repositoryWrapper,
        IMapper mapper,
        ILoggerService logger)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<IEnumerable<TimelineItemDTO>>> Handle(GetTimelineItemsByStreetcodeIdQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<DAL.Entities.Timeline.TimelineItem, bool>>? basePredicate = tl => tl.StreetcodeId == request.StreetcodeId;
        var predicate = basePredicate.ExtendWithAccessPredicate(new StreetcodeAccessManager(), request.UserRole, tl => tl.Streetcode);

        var timelineItems = await _repositoryWrapper.TimelineRepository
            .GetAllAsync(
                predicate: predicate,
                include: ti => ti
                    .Include(til => til.HistoricalContextTimelines)
                        .ThenInclude(x => x.HistoricalContext) !);

        if (!timelineItems.Any())
        {
            string message = "Returning empty enumerable of timeline items";
            _logger.LogInformation(message);
            return Result.Ok(Enumerable.Empty<TimelineItemDTO>());
        }

        return Result.Ok(_mapper.Map<IEnumerable<TimelineItemDTO>>(timelineItems));
    }
}
