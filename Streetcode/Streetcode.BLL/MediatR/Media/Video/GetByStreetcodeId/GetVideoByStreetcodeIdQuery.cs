using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Media.Video;

namespace Streetcode.BLL.MediatR.Media.Video.GetByStreetcodeId;

public record GetVideoByStreetcodeIdQuery(int StreetcodeId) : IRequest<Result<VideoDTO>>;