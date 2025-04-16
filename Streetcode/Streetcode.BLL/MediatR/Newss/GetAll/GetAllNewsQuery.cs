using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.News;

namespace Streetcode.BLL.MediatR.Newss.GetAll
{
    public record GetAllNewsQuery(ushort? page, ushort? pageSize, string? title = null)
        : IRequest<Result<GetAllNewsResponseDTO>>;
}
