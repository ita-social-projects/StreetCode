using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode.TextContent.Text;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.MediatR.Streetcode.Text.GetByStreetcodeId;

public record GetTextByStreetcodeIdQuery(int StreetcodeId, UserRole? UserRole)
    : IRequest<Result<TextDTO>>;