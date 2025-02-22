using FluentResults;
using MediatR;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.GetUrlByQrId
{
    public record GetStreetcodeUrlByQrIdQuery(int qrId, UserRole? userRole)
        : IRequest<Result<string>>;
}
