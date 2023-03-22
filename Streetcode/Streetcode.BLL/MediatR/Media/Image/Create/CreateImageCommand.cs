using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Media.Images;

namespace Streetcode.BLL.MediatR.Media.Image.Create;

public record CreateImageCommand(ImageFileBaseCreateDTO Image) : IRequest<Result<ImageDTO>>;
