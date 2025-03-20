using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Timeline;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.MediatR.Timeline.HistoricalContext.GetByTitle;

public record GetHistoricalContextByTitleQuery(string Title, UserRole? UserRole)
    : IRequest<Result<HistoricalContextDTO>>;