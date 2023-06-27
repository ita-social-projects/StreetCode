using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Media.Video;

namespace Streetcode.BLL.MediatR.Media.Video.GetById;

public record GetVideoByIdQuery(int Id) : IRequest<Result<VideoDTO>>;
