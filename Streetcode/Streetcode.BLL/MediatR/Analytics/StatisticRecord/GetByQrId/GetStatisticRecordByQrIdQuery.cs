using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Analytics;

namespace Streetcode.BLL.MediatR.Analytics.StatisticRecord.GetByQrId
{
    public record GetStatisticRecordByQrIdQuery(int qrId)
        : IRequest<Result<StatisticRecordDTO>>;
}
