using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Media;

namespace Streetcode.BLL.MediatR.Media.Audio.GetByStreetcodeId;

public record GetAudioByStreetcodeIdQuery(int StreetcodeId) : IRequest<Result<AudioDTO>>;