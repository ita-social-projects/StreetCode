using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.DTO.Timeline;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Timeline.TimelineItem.GetByStreetcodeId;

public class GetTimelineItemByStreetcodeIdHandler : IRequestHandler<GetTimelineItemByStreetcodeIdQuery, Result<IEnumerable<TimelineItemDTO>>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public GetTimelineItemByStreetcodeIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<TimelineItemDTO>>> Handle(GetTimelineItemByStreetcodeIdQuery request, CancellationToken cancellationToken)
    {
        var timelineItem = await _repositoryWrapper.TimelineRepository
            .GetAllAsync(
                predicate: f => f.Streetcodes.Any(s => s.Id == request.StreetcodeId),
                include: ti => ti.Include(til => til.HistoricalContexts));

        if (timelineItem is null)
        {
            return Result.Fail(new Error($"Cannot find a timeline item by a streetcode id: {request.StreetcodeId}"));
        }

        var timelineItemDto = _mapper.Map<IEnumerable<TimelineItemDTO>>(timelineItem);
        return Result.Ok(timelineItemDto);
    }
}