using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Team;

namespace Streetcode.BLL.MediatR.Team.Position.Update;

public record UpdateTeamPositionCommand(PositionDTO positionDto) : IRequest<Result<PositionDTO>>;
