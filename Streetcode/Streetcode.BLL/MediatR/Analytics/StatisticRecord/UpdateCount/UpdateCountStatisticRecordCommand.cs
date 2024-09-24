using FluentResults;
using MediatR;

namespace Streetcode.BLL.MediatR.Analytics.StatisticRecord.UpdateCount
{
    public record UpdateCountStatisticRecordCommand(int qrId)
        : IRequest<Result<Unit>>;
}
