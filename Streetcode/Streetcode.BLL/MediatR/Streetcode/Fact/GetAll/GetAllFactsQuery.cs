using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode.TextContent;

namespace Streetcode.BLL.MediatR.Streetcode.Fact.GetAll;

public record GetAllFactsQuery : IRequest<Result<IEnumerable<FactDTO>>>;