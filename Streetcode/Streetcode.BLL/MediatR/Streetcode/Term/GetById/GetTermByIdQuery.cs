using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode.TextContent.Term;

namespace Streetcode.BLL.MediatR.Streetcode.Term.GetById;

public record GetTermByIdQuery(int Id)
    : IRequest<Result<TermDto>>;
