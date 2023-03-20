using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Media;
using Streetcode.BLL.DTO.Media.Images;

namespace Streetcode.BLL.MediatR.Media.Image.UploadBase;

public record UploadBaseImageCommand(FileBaseCreateDTO Image) : IRequest<Result<Unit>>;
