using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Media;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.DTO.Streetcode.TextContent;

namespace Streetcode.BLL.MediatR.Media.Image.GetAll;

public record GetAllImagesQuery : IRequest<Result<IEnumerable<ImageDTO>>>;