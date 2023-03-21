using FluentResults;
using MediatR;

namespace Streetcode.BLL.MediatR.Media.Audio.Delete;

public record DeleteAudioCommand(int Id) : IRequest<Result<Unit>>;
