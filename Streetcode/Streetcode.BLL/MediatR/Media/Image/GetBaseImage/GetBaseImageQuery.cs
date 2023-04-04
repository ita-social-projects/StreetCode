using FluentResults;
using MediatR;

namespace Streetcode.BLL.MediatR.Media.Image.GetBaseImage;

public record GetBaseImageQuery(int Id) : IRequest<Result<MemoryStream>>;