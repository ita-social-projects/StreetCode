using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Team;

namespace Streetcode.BLL.MediatR.Team.Position.GetById;

public record GetByIdTeamPositionQuery(int positionId)
    : IRequest<Result<PositionDTO>>;
