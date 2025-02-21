using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.News;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.MediatR.Newss.GetByUrl
{
    public record GetNewsByUrlQuery(string url, UserRole? userRole)
        : IRequest<Result<NewsDTO>>;
}
