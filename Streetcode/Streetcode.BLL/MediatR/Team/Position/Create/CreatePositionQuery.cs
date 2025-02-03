using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Team;

namespace Streetcode.BLL.MediatR.Team.Create
{
    public record CreatePositionQuery(PositionCreateDto position)
        : IRequest<Result<PositionDto>>;
}
