using FluentResults;
using MediatR;

namespace Streetcode.BLL.MediatR.Analytics.StatisticRecord.ExistByQrId
{
    public record ExistStatisticRecordByQrIdCommand(int qrId): IRequest<Result<bool>>
    {
    }
}
