using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Analytics;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.MediatR.Analytics.StatisticRecord.GetByQrId
{
    public record GetStatisticRecordByQrIdQuery(int QrId, UserRole? UserRole)
        : IRequest<Result<StatisticRecordDTO>>;
}
