using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Analytics;

namespace Streetcode.BLL.MediatR.Analytics.StatisticRecord.GetAll
{
    public record GetAllStatisticRecordsQuery() : IRequest<Result<IEnumerable<StatisticRecordDTO>>>
    {
    }
}
