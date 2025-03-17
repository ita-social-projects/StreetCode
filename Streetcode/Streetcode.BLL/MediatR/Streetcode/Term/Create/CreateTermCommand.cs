using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode.TextContent.Term;

namespace Streetcode.BLL.MediatR.Streetcode.Term.Create;

public record CreateTermCommand(TermCreateDto Term)
    : IRequest<Result<TermDto>>;
