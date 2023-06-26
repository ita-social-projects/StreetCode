using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Media.Video;

namespace Streetcode.BLL.MediatR.Media.Video.GetAll;

public record GetAllVideosQuery : IRequest<Result<IEnumerable<VideoDTO>>>;