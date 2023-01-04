using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.Create;

public record CreateStreetcodeCommand(StreetcodeDTO Streetcode) : IRequest<Result<Unit>>;