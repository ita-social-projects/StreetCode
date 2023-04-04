using FluentResults;
using MediatR;

namespace Streetcode.BLL.MediatR.Streetcode.RelatedTerm.Delete
{
    public record DeleteRelatedTermCommand(int id) : IRequest<Result<Unit>>;
}
