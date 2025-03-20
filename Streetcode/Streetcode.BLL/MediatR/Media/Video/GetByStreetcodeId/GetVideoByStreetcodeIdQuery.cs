using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Media.Video;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.MediatR.Media.Video.GetByStreetcodeId;

public record GetVideoByStreetcodeIdQuery(int StreetcodeId, UserRole? UserRole)
    : IRequest<Result<VideoDTO>>;