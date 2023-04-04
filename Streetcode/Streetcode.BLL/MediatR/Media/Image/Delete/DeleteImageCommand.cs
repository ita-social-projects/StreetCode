using FluentResults;
using MediatR;

namespace Streetcode.BLL.MediatR.Media.Image.Delete;

public record DeleteImageCommand(int Id) : IRequest<Result<Unit>>;
