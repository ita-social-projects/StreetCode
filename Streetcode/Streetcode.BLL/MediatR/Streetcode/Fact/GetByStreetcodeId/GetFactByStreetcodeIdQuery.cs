using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode.TextContent.Fact;

namespace Streetcode.BLL.MediatR.Streetcode.Fact.GetByStreetcodeId;

public record GetFactByStreetcodeIdQuery(int StreetcodeId) : IRequest<Result<IEnumerable<FactDto>>>;