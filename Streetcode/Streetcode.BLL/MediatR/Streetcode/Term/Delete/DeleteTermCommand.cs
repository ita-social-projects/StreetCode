using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode.TextContent;

namespace Streetcode.BLL.MediatR.Streetcode.Term.Delete
{
    public record DeleteTermCommand(int id) : IRequest<Result<Unit>>;
}
