using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Media;

namespace Streetcode.BLL.MediatR.Media.Audio.CreateNewAudio;

public record CreateAudioCommand(AudioFileBaseCreateDTO Audio) : IRequest<Result<AudioDTO>>;
