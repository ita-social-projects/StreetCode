using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.DTO.Streetcode.Create;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.Create;
public record class CreateStreetcodeCommand(StreetcodeCreateDTO Streetcode): IRequest<Result<int>>;
