using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Media.Images;

namespace Streetcode.BLL.MediatR.Media.Image.GetAll;

public record GetAllImagesQuery : IRequest<Result<IEnumerable<ImageDTO>>>;