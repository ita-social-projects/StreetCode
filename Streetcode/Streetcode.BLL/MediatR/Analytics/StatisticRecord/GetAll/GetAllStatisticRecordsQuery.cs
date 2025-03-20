using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Analytics;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.MediatR.Analytics.StatisticRecord.GetAll
{
    public record GetAllStatisticRecordsQuery(UserRole? UserRole)
        : IRequest<Result<IEnumerable<StatisticRecordDTO>>>;
}
