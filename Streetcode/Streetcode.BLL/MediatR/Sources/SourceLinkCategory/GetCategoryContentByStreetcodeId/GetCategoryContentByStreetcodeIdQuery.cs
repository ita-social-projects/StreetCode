using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.DTO.Sources;

namespace Streetcode.BLL.MediatR.Sources.SourceLinkCategory.GetCategoryContentByStreetcodeId
{
    public record GetCategoryContentByStreetcodeIdQuery(int streetcodeId, int categoryId) : IRequest<Result<StreetcodeCategoryContentDTO>>;
}
