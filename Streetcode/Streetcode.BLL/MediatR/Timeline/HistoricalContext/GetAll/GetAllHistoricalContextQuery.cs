using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.DTO.Timeline;

namespace Streetcode.BLL.MediatR.Timeline.HistoricalContext.GetAll
{
    public record GetAllHistoricalContextQuery(ushort? page = null, ushort? pageSize = null): IRequest<Result<GetAllHistoricalContextDTO>>;
}
