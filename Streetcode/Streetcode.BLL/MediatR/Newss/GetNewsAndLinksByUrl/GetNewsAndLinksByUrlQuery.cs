using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.News;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.MediatR.Newss.GetNewsAndLinksByUrl
{
    public record GetNewsAndLinksByUrlQuery(string url, UserRole? userRole)
        : IRequest<Result<NewsDTOWithURLs>>;
}
