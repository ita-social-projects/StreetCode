using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Media.Images;

namespace Streetcode.BLL.MediatR.Media.Image.Update;

public record UpdateImageCommand(ImageFileBaseUpdateDto Image)
    : IRequest<Result<ImageDto>>;