using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Sources;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.MediatR.Sources.SourceLinkCategory.GetCategoryContentByStreetcodeId
{
    public record GetCategoryContentByStreetcodeIdQuery(int StreetcodeId, int CategoryId, UserRole? UserRole)
        : IRequest<Result<StreetcodeCategoryContentDTO>>;
}
