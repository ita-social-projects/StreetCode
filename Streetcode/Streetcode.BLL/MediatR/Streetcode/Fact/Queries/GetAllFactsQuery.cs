using MediatR;
using Streetcode.BLL.DTO.Streetcode.TextContent;

namespace Streetcode.BLL.MediatR.Streetcode.Fact.Queries
{
    public record GetAllFactsQuery() : IRequest<IEnumerable<FactDTO>>;
}
