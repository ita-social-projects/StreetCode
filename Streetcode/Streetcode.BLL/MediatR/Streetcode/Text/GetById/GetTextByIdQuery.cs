using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode.TextContent.Text;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.MediatR.Streetcode.Text.GetById;

public record GetTextByIdQuery(int Id, UserRole? UserRole)
    : IRequest<Result<TextDTO>>;
