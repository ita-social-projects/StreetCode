using FluentResults;
using MediatR;

namespace Streetcode.BLL.MediatR.Team.GetNamePosition
{
    public record GetTickerStringQuery()
        : IRequest<Result<string>>;
}
