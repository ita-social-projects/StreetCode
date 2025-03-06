using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.News;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.MediatR.Newss.GetNewsAndLinksByUrl
{
    public record GetNewsAndLinksByUrlQuery(string Url, UserRole? UserRole)
        : IRequest<Result<NewsDTOWithURLs>>;
}
