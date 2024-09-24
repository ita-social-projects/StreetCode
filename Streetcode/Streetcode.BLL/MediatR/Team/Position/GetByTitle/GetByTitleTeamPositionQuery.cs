using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Team;

namespace Streetcode.BLL.MediatR.Team.Position.GetByTitle;

public record GetByTitleTeamPositionQuery(string position)
    : IRequest<Result<PositionDTO>>;
