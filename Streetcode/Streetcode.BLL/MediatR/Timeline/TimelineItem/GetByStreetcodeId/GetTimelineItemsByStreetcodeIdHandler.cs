using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Timeline;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Timeline.TimelineItem.GetByStreetcodeId;

public class GetTimelineItemsByStreetcodeIdHandler : IRequestHandler<GetTimelineItemsByStreetcodeIdQuery, Result<IEnumerable<TimelineItemDto>>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ILoggerService _logger;
    private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFind;

    public GetTimelineItemsByStreetcodeIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger, IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _logger = logger;
        _stringLocalizerCannotFind = stringLocalizerCannotFind;
    }

    public async Task<Result<IEnumerable<TimelineItemDto>>> Handle(GetTimelineItemsByStreetcodeIdQuery request, CancellationToken cancellationToken)
    {
        var timelineItems = await _repositoryWrapper.TimelineRepository
            .GetAllAsync(
                predicate: f => f.StreetcodeId == request.StreetcodeId,
                include: ti => ti
                    .Include(til => til.HistoricalContextTimelines)
                        .ThenInclude(x => x.HistoricalContext) !);

        if (!timelineItems.Any())
        {
            string message = "Returning empty enumerable of timeline items";
            _logger.LogInformation(message);
            return Result.Ok(Enumerable.Empty<TimelineItemDto>());
        }

        return Result.Ok(_mapper.Map<IEnumerable<TimelineItemDto>>(timelineItems));
    }
}
