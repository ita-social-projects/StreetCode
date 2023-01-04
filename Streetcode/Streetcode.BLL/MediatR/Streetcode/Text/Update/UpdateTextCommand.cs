using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode.TextContent;

namespace Streetcode.BLL.MediatR.Streetcode.Text.Update;

public record UpdateTextCommand(TextDTO Text) : IRequest<Result<Unit>>;