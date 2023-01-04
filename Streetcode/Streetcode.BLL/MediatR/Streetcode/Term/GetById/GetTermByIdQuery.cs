using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode.TextContent;

namespace Streetcode.BLL.MediatR.Streetcode.Term.GetById;

public record GetTermByIdQuery(int Id) : IRequest<Result<TermDTO>>;
