using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Sources;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.MediatR.Sources.SourceLink.GetCategoriesByStreetcodeId;

public record GetCategoriesByStreetcodeIdQuery(int StreetcodeId, UserRole? UserRole)
    : IRequest<Result<IEnumerable<SourceLinkCategoryDTO>>>;