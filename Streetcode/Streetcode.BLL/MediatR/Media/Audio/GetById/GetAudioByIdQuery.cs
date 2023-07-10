using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Media.Audio;

namespace Streetcode.BLL.MediatR.Media.Audio.GetById;

public record GetAudioByIdQuery(int Id) : IRequest<Result<AudioDTO>>;
