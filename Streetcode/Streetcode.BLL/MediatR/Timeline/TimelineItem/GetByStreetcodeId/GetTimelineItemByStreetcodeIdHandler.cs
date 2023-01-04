using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Timeline;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Timeline.TimelineItem.GetByStreetcodeId;

public class GetTimelineItemByStreetcodeIdHandler : IRequestHandler<GetTimelineItemByStreetcodeIdQuery, Result<TimelineItemDTO>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public GetTimelineItemByStreetcodeIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
    }

    public async Task<Result<TimelineItemDTO>> Handle(GetTimelineItemByStreetcodeIdQuery request, CancellationToken cancellationToken)
    {
        var timelineItem = await _repositoryWrapper.TimelineRepository
            .GetSingleOrDefaultAsync(f => f.Streetcodes.Any(s => s.Id == request.StreetcodeId));

        if (timelineItem is null)
        {
            return Result.Fail(new Error($"Cannot find a timelineItem by a streetcode Id: {request.StreetcodeId}"));
        }

        var timelineItemDto = _mapper.Map<TimelineItemDTO>(timelineItem);
        return Result.Ok(timelineItemDto);
    }
}