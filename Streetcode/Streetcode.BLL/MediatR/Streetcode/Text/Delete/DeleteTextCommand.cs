using FluentResults;
using MediatR;

namespace Streetcode.BLL.MediatR.Streetcode.Text.Delete;

public record DeleteTextCommand(int Id): IRequest<Result<Unit>>;