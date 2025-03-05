using FluentResults;
using MediatR;

namespace Streetcode.BLL.MediatR.Streetcode.Fact.Delete;

public record DeleteFactCommand(int Id)
    : IRequest<Result<Unit>>;
