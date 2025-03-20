using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Timeline;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.MediatR.Timeline.TimelineItem.GetAll;

public record GetAllTimelineItemsQuery(UserRole? UserRole) : IRequest<Result<IEnumerable<TimelineItemDTO>>>;