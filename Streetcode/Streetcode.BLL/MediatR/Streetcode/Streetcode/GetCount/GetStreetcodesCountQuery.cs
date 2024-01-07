using FluentResults;
using MediatR;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.GetCount
{
    public record GetStreetcodesCountQuery : IRequest<Result<int>>;
}
