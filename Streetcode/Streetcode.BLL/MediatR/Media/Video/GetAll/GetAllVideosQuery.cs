using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Media.Video;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.MediatR.Media.Video.GetAll;

public record GetAllVideosQuery(UserRole? UserRole) : IRequest<Result<IEnumerable<VideoDTO>>>;