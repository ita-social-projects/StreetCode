using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Sources;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.MediatR.Sources.SourceLinkCategory.GetAll
{
    public record GetAllCategoryNamesQuery(UserRole? UserRole) : IRequest<Result<IEnumerable<CategoryWithNameDTO>>>;
}
