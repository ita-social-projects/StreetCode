using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.DTO.Timeline;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Timeline.TimelineItem.GetByStreetcodeId;

public class GetTimelineItemsByStreetcodeIdHandler : IRequestHandler<GetTimelineItemsByStreetcodeIdQuery, Result<IEnumerable<TimelineItemDTO>>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public GetTimelineItemsByStreetcodeIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<TimelineItemDTO>>> Handle(GetTimelineItemsByStreetcodeIdQuery request, CancellationToken cancellationToken)
    {
        var timelineItems = await _repositoryWrapper.TimelineRepository
            .GetAllAsync(
                predicate: f => f.Streetcodes.Any(s => s.Id == request.StreetcodeId),
                include: ti => ti.Include(til => til.HistoricalContexts));

        if (timelineItems is null)
        {
            return Result.Fail(new Error($"Cannot find any timeline item by the streetcode id: {request.StreetcodeId}"));
        }

        var timelineItemDto = _mapper.Map<IEnumerable<TimelineItemDTO>>(timelineItems);
        return Result.Ok(timelineItemDto);
    }
}