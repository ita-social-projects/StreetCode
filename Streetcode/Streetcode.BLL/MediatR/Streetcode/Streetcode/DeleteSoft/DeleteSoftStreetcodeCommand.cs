using FluentResults;
using MediatR;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.DeleteSoft;

public record DeleteSoftStreetcodeCommand(int Id) : IRequest<Result<Unit>>;
