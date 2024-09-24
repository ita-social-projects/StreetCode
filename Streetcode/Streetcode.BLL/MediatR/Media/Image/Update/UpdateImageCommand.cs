using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Media;
using Streetcode.BLL.DTO.Media.Images;

namespace Streetcode.BLL.MediatR.Media.Image.Update;

public record UpdateImageCommand(ImageFileBaseUpdateDTO Image)
    : IRequest<Result<ImageDTO>>;