using FluentResults;
using MediatR;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.MediatR.Media.Audio.GetBaseAudio;

public record GetBaseAudioQuery(int Id, UserRole? UserRole)
    : IRequest<Result<MemoryStream>>;
