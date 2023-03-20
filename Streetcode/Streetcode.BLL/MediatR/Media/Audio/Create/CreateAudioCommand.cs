using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Media;

namespace Streetcode.BLL.MediatR.Media.Audio.CreateNewAudio;

public record CreateAudioCommand(FileBaseCreateDTO Audio) : IRequest<Result<Unit>>;
