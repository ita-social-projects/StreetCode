using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode.TextContent;

namespace Streetcode.BLL.MediatR.Streetcode.Fact.GetById;

public record GetFactByIdQuery(int Id) : IRequest<Result<FactDTO>>;
