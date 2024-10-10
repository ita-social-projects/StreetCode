using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Team;

namespace Streetcode.BLL.MediatR.Team.Position.GetAll
{
    public record GetAllPositionsQuery(ushort? page = null, ushort? pageSize = null)
        : IRequest<Result<GetAllPositionsDTO>>;
}
