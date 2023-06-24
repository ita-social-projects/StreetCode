using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.DTO.AdditionalContent.Subtitles;
using Streetcode.BLL.DTO.Timeline;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Timeline.TimelineItem.GetByStreetcodeId;

public class GetTimelineItemsByStreetcodeIdHandler : IRequestHandler<GetTimelineItemsByStreetcodeIdQuery, Result<IEnumerable<TimelineItemDTO>>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ILoggerService? _logger;

    public GetTimelineItemsByStreetcodeIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService? logger = null)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<IEnumerable<TimelineItemDTO>>> Handle(GetTimelineItemsByStreetcodeIdQuery request, CancellationToken cancellationToken)
    {
        var timelineItems = await _repositoryWrapper.TimelineRepository
            .GetAllAsync(
                predicate: f => f.StreetcodeId == request.StreetcodeId,
                include: ti => ti
                    .Include(til => til.HistoricalContextTimelines)
                        .ThenInclude(x => x.HistoricalContext)!);

        if (timelineItems is null)
        {
            string errorMsg = $"Cannot find any timeline item by the streetcode id: {request.StreetcodeId}";
            _logger?.LogError("GetTimelineItemsByStreetcodeIdQuery handled with an error");
            _logger?.LogError(errorMsg);
            return Result.Fail(new Error(errorMsg));
        }

        var timelineItemDto = _mapper.Map<IEnumerable<TimelineItemDTO>>(timelineItems);
        _logger?.LogInformation($"GetTimelineItemsByStreetcodeIdQuery handled successfully");
        _logger?.LogInformation($"Retrieved {timelineItemDto.Count()} timelineItems");
        return Result.Ok(timelineItemDto);
    }
}
