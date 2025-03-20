using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Media.Audio;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.MediatR.Media.Audio.GetById;

public record GetAudioByIdQuery(int Id, UserRole? UserRole)
    : IRequest<Result<AudioDTO>>;
