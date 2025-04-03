using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Media.Video;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.MediatR.Media.Video.GetById;

public record GetVideoByIdQuery(int Id, UserRole? UserRole)
    : IRequest<Result<VideoDTO>>;
