using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.News;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.MediatR.Newss.GetById
{
    public record GetNewsByIdQuery(int Id, UserRole? UserRole)
        : IRequest<Result<NewsDTO>>;
}
