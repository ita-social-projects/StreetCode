using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.MediatR.Media.Image.GetByStreetcodeId;

public record GetImageByStreetcodeIdQuery(int StreetcodeId, UserRole? UserRole)
    : IRequest<Result<IEnumerable<ImageDTO>>>;