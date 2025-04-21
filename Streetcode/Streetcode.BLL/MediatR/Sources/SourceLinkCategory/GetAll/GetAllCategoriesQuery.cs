using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Sources;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.MediatR.Sources.SourceLinkCategory.GetAll
{
    public record GetAllCategoriesQuery(UserRole? UserRole, ushort? page, ushort? pageSize, string? title = null)
        : IRequest<Result<GetAllCategoriesResponseDTO>>;
}
