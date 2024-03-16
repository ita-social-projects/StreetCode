using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.News;

namespace Streetcode.BLL.MediatR.Newss.SortedByDateTime
{
    public record SortedByDateTimeQuery(ushort page, ushort pageSize) : IRequest<Result<IEnumerable<NewsDTO>>>;
}
