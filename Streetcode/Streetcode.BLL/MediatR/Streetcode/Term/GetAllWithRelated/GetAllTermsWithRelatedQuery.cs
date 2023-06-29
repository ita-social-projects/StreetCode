using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode.TextContent;

namespace Streetcode.BLL.MediatR.Streetcode.Term.GetAllWithRelated
{
    public record GetAllTermsWithRelatedQuery : IRequest<Result<IEnumerable<TermDTO>>>;
}
