using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Media.Images;

namespace Streetcode.BLL.MediatR.Media.Image.UploadBase;

public record UploadBaseImageCommand(ImageBaseDTO Image) : IRequest<Result<Unit>>;
