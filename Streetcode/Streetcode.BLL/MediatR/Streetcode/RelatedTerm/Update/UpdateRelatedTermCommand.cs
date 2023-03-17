using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode.TextContent;

namespace Streetcode.BLL.MediatR.Streetcode.RelatedTerm.Update
{
    public record UpdateRelatedTermCommand(int id, RelatedTermDTO RelatedTerm) : IRequest<Result<Unit>>
    {
    }
}
