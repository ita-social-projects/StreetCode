using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Timeline;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.MediatR.Timeline.TimelineItem.GetByStreetcodeId;

public record GetTimelineItemsByStreetcodeIdQuery(int StreetcodeId, UserRole? UserRole)
    : IRequest<Result<IEnumerable<TimelineItemDTO>>>;