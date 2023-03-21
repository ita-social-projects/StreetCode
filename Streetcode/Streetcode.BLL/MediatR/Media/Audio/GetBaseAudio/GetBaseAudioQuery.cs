using FluentResults;
using MediatR;

namespace Streetcode.BLL.MediatR.Media.Audio.GetBaseAudio;

public record GetBaseAudioQuery(int Id) : IRequest<Result<MemoryStream>>;
