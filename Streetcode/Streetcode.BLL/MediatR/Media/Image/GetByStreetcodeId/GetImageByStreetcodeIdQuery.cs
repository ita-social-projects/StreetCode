using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Media.Images;

namespace Streetcode.BLL.MediatR.Media.Image.GetByStreetcodeId;

public record GetImageByStreetcodeIdQuery(int streetcodeId) : IRequest<Result<IEnumerable<ImageDTO>>>;