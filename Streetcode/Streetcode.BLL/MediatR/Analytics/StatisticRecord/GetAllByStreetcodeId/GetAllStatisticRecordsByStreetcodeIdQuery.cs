using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Analytics;

namespace Streetcode.BLL.MediatR.Analytics.StatisticRecord.GetAllByStreetcodeId
{
    public record GetAllStatisticRecordsByStreetcodeIdQuery(int streetcodeId)
        : IRequest<Result<IEnumerable<StatisticRecordDto>>>;
}
