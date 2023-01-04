using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Media;
using Streetcode.BLL.DTO.Streetcode.TextContent;

namespace Streetcode.BLL.MediatR.Media.Video.GetAll;

public record GetAllVideosQuery : IRequest<Result<IEnumerable<VideoDTO>>>;