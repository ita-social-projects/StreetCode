using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Sources;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.MediatR.Sources.SourceLinkCategory.GetAll
{
    public record GetAllCategoriesQuery(UserRole? UserRole, ushort? Page, ushort? PageSize)
        : IRequest<Result<GetAllCategoriesResponseDTO>>;
}
