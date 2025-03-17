using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode.TextContent.Term;

namespace Streetcode.BLL.MediatR.Streetcode.Term.Update;

public record UpdateTermCommand(TermDto Term)
    : IRequest<Result<Unit>>;
