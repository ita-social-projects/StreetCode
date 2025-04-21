using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Timeline;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.MediatR.Timeline.TimelineItem.GetById;

public record GetTimelineItemByIdQuery(int Id, UserRole? UserRole)
    : IRequest<Result<TimelineItemDTO>>;
