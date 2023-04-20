using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Sources;

namespace Streetcode.BLL.MediatR.Sources.SourceLink.GetSubCategoriesByCategoryId;

public record GetSubCategoriesByCategoryIdQuery(int CategoryId) : IRequest<Result<IEnumerable<SourceLinkSubCategoryDTO>>>;
