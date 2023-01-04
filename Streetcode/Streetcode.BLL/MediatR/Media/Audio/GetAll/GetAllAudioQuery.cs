using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Media;
using Streetcode.BLL.DTO.Streetcode.TextContent;

namespace Streetcode.BLL.MediatR.Media.Audio.GetAll;

public record GetAllAudeoQuery : IRequest<Result<IEnumerable<AudioDTO>>>;