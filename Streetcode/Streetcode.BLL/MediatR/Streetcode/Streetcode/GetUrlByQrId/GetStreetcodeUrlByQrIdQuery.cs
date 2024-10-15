using FluentResults;
using MediatR;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.GetUrlByQrId
{
    public record GetStreetcodeUrlByQrIdQuery(int qrId)
        : IRequest<Result<string>>;
}
