using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.News;

namespace Streetcode.BLL.MediatR.Newss.GetById
{
    public record GetNewsByIdQuery(int id) : IRequest<Result<NewsDTO>>;
}
