using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Timeline;

namespace Streetcode.BLL.MediatR.Timeline.TimelineItem.GetByStreetcodeId;

public record GetTransactLinkByStreetcodeIdQuery(int StreetcodeId) : IRequest<Result<TimelineItemDTO>>;