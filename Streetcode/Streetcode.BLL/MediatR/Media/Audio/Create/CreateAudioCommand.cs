using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Media;

namespace Streetcode.BLL.MediatR.Media.Audio.Create;

public record CreateAudioCommand(AudioFileBaseCreateDTO Audio) : IRequest<Result<AudioDTO>>;
