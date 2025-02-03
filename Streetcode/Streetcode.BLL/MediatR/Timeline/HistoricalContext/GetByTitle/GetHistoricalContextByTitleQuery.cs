using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Timeline;

namespace Streetcode.BLL.MediatR.Timeline.HistoricalContext.GetByTitle;

public record GetHistoricalContextByTitleQuery(string title)
    : IRequest<Result<HistoricalContextDto>>;