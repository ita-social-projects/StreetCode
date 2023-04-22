using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Sources;

namespace Streetcode.BLL.MediatR.Sources.SourceLink.GetCategoriesByStreetcodeId;

public record GetCategoriesByStreetcodeIdQuery(int StreetcodeId) : IRequest<Result<IEnumerable<SourceLinkCategoryDTO>>>;