using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.News;
using Streetcode.DAL.Entities.News;

namespace Streetcode.BLL.MediatR.Newss.Update
{
    public record UpdateNewsCommand(NewsDTO news) : IRequest<Result<NewsDTO>>;
}
