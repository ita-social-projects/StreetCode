using FluentResults;
using MediatR;

namespace Streetcode.BLL.MediatR.Team.Position.Delete;

public record DeleteTeamPositionCommand(int positionId) : IRequest<Result<int>>;
