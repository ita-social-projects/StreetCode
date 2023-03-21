using FluentResults;
using MediatR;

namespace Streetcode.BLL.MediatR.Media.Audio.GetBaseAudio;

public record GetBaseAudioQuery(string Name) : IRequest<Result<MemoryStream>>;
