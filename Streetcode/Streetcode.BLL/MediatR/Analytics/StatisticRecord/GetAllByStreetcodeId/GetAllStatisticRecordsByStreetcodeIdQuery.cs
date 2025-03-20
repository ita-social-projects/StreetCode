using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Analytics;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.MediatR.Analytics.StatisticRecord.GetAllByStreetcodeId
{
    public record GetAllStatisticRecordsByStreetcodeIdQuery(int StreetcodeId, UserRole? UserRole)
        : IRequest<Result<IEnumerable<StatisticRecordDTO>>>;
}
