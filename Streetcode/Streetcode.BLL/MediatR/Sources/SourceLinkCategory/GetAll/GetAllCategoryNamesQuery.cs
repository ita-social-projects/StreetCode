using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.DTO.Sources;

namespace Streetcode.BLL.MediatR.Sources.SourceLinkCategory.GetAll
{
    public record GetAllCategoryNamesQuery : IRequest<Result<IEnumerable<CategoryWithNameDTO>>>;
}
