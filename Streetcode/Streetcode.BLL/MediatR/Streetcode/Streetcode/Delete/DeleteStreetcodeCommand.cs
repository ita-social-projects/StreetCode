using FluentResults;
using MediatR;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.Delete;

public record DeleteStreetcodeCommand(int Id)
    : IRequest<Result<Unit>>;