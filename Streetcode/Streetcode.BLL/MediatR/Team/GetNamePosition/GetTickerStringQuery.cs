using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Team;

namespace Streetcode.BLL.MediatR.Team.GetNamePosition
{
    public record GetTickerStringQuery()
        : IRequest<Result<string>>;
}
