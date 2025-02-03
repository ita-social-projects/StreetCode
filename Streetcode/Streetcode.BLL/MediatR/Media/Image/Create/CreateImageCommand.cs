using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Media.Images;

namespace Streetcode.BLL.MediatR.Media.Image.Create;

public record CreateImageCommand(ImageFileBaseCreateDto Image)
    : IRequest<Result<ImageDto>>;
