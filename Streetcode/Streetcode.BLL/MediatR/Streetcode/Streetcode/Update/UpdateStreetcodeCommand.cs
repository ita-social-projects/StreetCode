using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.DTO.Streetcode.TextContent;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.Update;

public record UpdateStreetcodeCommand(StreetcodeDTO Fact) : IRequest<Result<Unit>>;