using FluentResults;
using MediatR;

namespace Streetcode.BLL.MediatR.Streetcode.RelatedTerm.Delete
{
    public record DeleteRelatedTermCommand(string word) : IRequest<Result<DAL.Entities.Streetcode.TextContent.RelatedTerm>>;
}
