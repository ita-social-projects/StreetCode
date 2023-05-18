using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.News;

namespace Streetcode.BLL.MediatR.Newss.GetAll
{
    public record GetAllNewsQuery() : IRequest<Result<IEnumerable<NewsDTO>>>;
}
