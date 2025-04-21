using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.News;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.MediatR.Newss.GetAll
{
    public record GetAllNewsQuery(ushort? page, ushort? pageSize, UserRole? UserRole, string? title = null)
        : IRequest<Result<GetAllNewsResponseDTO>>;
}
