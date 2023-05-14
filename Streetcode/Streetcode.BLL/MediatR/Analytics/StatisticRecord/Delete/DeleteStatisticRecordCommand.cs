using FluentResults;
using MediatR;

namespace Streetcode.BLL.MediatR.Analytics.StatisticRecord.Delete
{
    public record DeleteStatisticRecordCommand(int qrId) : IRequest<Result<Unit>>
    {
    }
}
