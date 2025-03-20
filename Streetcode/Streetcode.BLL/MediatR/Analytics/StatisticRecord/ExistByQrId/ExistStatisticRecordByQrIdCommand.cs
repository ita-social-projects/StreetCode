using FluentResults;
using MediatR;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.MediatR.Analytics.StatisticRecord.ExistByQrId
{
    public record ExistStatisticRecordByQrIdCommand(int QrId, UserRole? UserRole)
        : IRequest<Result<bool>>;
}
