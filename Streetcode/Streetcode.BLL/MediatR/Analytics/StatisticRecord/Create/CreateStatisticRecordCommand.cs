using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Analytics;

namespace Streetcode.BLL.MediatR.Analytics.StatisticRecord.Create
{
    public record CreateStatisticRecordCommand(StatisticRecordDto StatisticRecordDTO)
        : IRequest<Result<StatisticRecordResponseDto>>;
}
