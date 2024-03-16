using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.News;

namespace Streetcode.BLL.MediatR.Newss.GetAllPaginated
{
    public record GetAllNewsPaginatedQuery(ushort page, ushort pageSize) : IRequest<Result<IEnumerable<NewsDTO>>>;
}
