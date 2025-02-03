using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode.TextContent;

namespace Streetcode.BLL.MediatR.Streetcode.RelatedTerm.Create
{
    public record CreateRelatedTermCommand(RelatedTermCreateDto RelatedTerm)
        : IRequest<Result<RelatedTermDto>>;
}
