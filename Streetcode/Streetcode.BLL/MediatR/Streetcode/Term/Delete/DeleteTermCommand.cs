using FluentResults;
using MediatR;

namespace Streetcode.BLL.MediatR.Streetcode.Term.Delete;

public record DeleteTermCommand(int id)
    : IRequest<Result<Unit>>;
