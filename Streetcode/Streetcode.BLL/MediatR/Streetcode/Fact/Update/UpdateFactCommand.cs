using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode.TextContent;

namespace Streetcode.BLL.MediatR.Streetcode.Fact.Update;

public record UpdateFactCommand(FactDTO Fact) : IRequest<Result<Unit>>;