using FluentResults;
using MediatR;

namespace Streetcode.BLL.MediatR.Newss.Delete
{
    public record DeleteNewsCommand(int id) : IRequest<Result<Unit>>;
}
