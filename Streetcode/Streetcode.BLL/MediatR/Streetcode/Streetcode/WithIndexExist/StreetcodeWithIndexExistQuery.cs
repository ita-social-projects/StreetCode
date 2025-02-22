using FluentResults;
using MediatR;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.WithIndexExist
{
    public record StreetcodeWithIndexExistQuery(int index, UserRole? userRole)
        : IRequest<Result<bool>>;
}
