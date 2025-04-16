using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Sources;

namespace Streetcode.BLL.MediatR.Sources.SourceLinkCategory.GetAll
{
    public record GetAllCategoriesQuery(ushort? page, ushort? pageSize, string? title = null)
        : IRequest<Result<GetAllCategoriesResponseDTO>>;
}
