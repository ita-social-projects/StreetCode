using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Timeline;

namespace Streetcode.BLL.MediatR.Timeline.TimelineItem.GetByStreetcodeId;

public record GetTimelineItemByStreetcodeIdQuery(int StreetcodeId) : IRequest<Result<TimelineItemDTO>>;