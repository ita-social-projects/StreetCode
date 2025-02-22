using FluentResults;
using MediatR;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.WithUrlExist
{
    public record StreetcodeWithUrlExistQuery(string url, UserRole? userRole)
        : IRequest<Result<bool>>;
}
