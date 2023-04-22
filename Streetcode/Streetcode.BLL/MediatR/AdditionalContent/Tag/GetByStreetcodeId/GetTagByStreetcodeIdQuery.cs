using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.AdditionalContent.Tag;

namespace Streetcode.BLL.MediatR.AdditionalContent.Tag.GetByStreetcodeId;

public record GetTagByStreetcodeIdQuery(int StreetcodeId) : IRequest<Result<IEnumerable<StreetcodeTagDTO>>>;