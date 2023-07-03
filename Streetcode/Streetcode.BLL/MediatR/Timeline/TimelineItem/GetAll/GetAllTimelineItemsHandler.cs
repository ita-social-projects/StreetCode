using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.DTO.Timeline;
using Streetcode.DAL.Entities.AdditionalContent.Coordinates;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Timeline.TimelineItem.GetAll;

public class GetAllTimelineItemsHandler : IRequestHandler<GetAllTimelineItemsQuery, Result<IEnumerable<TimelineItemDTO>>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public GetAllTimelineItemsHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<TimelineItemDTO>>> Handle(GetAllTimelineItemsQuery request, CancellationToken cancellationToken)
    {
        var timelineItems = await _repositoryWrapper
            .TimelineRepository
            .GetAllAsync(
                include: ti => ti.Include(til => til.HistoricalContexts));

        if (timelineItems is null)
        {
            return Result.Fail(new Error($"Cannot find any timelineItem"));
        }

        var timelineItemDtos = _mapper.Map<IEnumerable<TimelineItemDTO>>(timelineItems);
        return Result.Ok(timelineItemDtos);
    }
}