using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.News;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.MediatR.Newss.GetAll
{
    public record GetAllNewsQuery(ushort? page, ushort? pageSize, UserRole? userRole)
        : IRequest<Result<GetAllNewsResponseDTO>>;
}
