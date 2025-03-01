using FluentResults;
using MediatR;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.WithIndexExist
{
    public record StreetcodeWithIndexExistQuery(int Index)
        : IRequest<Result<bool>>;
}
