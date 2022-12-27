using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode.TextContent;

namespace Streetcode.BLL.MediatR.Streetcode.Fact.Create;

public record CreateFactCommand(FactDTO Fact) : IRequest<Result<Unit>>;