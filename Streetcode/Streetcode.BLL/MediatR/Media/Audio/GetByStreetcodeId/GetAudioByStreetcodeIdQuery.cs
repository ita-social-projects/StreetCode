using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Media.Audio;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.MediatR.Media.Audio.GetByStreetcodeId;

public record GetAudioByStreetcodeIdQuery(int StreetcodeId, UserRole? UserRole)
    : IRequest<Result<AudioDTO>>;