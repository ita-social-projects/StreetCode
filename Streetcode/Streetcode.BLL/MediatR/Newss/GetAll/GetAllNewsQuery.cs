using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.News;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.MediatR.Newss.GetAll
{
    public record GetAllNewsQuery(ushort? Page, ushort? PageSize, UserRole? UserRole)
        : IRequest<Result<GetAllNewsResponseDTO>>;
}
