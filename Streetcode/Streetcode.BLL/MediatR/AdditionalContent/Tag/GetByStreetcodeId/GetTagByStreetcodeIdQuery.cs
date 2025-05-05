using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.AdditionalContent.Tag;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.MediatR.AdditionalContent.Tag.GetByStreetcodeId;

public record GetTagByStreetcodeIdQuery(int StreetcodeId, UserRole? UserRole)
    : IRequest<Result<IEnumerable<StreetcodeTagDTO>>>;