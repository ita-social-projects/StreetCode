using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode.Create;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.Create;
public record class CreateStreetcodeCommand(StreetcodeCreateDto Streetcode)
    : IRequest<Result<int>>;
