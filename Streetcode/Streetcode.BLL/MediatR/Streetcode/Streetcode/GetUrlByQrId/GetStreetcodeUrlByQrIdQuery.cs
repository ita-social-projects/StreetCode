using FluentResults;
using MediatR;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.GetUrlByQrId;

public record GetStreetcodeUrlByQrIdQuery(int QrId)
    : IRequest<Result<string>>;